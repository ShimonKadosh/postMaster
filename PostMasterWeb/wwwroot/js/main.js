$(document).ready(function () {

    function closeDialog() {
        $("#black,#login,#signup,#post").removeClass("active");
    }

    $("#loginBtn,#signupBtn,#postBtn").click(function () {
        $("#black").addClass("active");
        var element = $(this).attr("id").replace("Btn", "");
        $("#" + element).addClass("active"); //#login, #signup or #post
    });

    $("#black,.closeDialog").click(function () {
        closeDialog();
    });

    var editing = false;
    var ptext = "";
    
    $(".edit").click(function () {
        if (!editing) {
            ptext = $(this).parent().parent().children("p").attr("contenteditable", true).focus().text();
            $(this).parent().children(".edit, .remove").addClass("hidden");
            $(this).parent().children(".ok, .cancel").removeClass("hidden");
            editing = true;
        }
    });


    $(".remove").click(function () {
        var button = this;
        var postId = $(this).data("postid");

        $.get("/api/posts/remove", { id: postId })
            .done(function (response) {
                if (response.status) {
                    $(button).parents("article").slideUp();

                    return true;
                }

                alert(reponse.errors[0]);
            }, "json");
    });

    $(".ok,.cancel").click(function () {
        if (!$(this).hasClass("ok")) {
            $(this).parents(".box-content").children("p").text(ptext);
        } else {
            var postId = $(this).data("postid");
            var postContent = $(this).parents(".box-content").children("p").text();

            $.ajax({
                type: "POST",
                dataType: "json",
                url: "/api/posts/update",
                data: {Id: postId, Content: postContent},
                success: function (response) {
                    if (!response.status) {
                        alert(response.errors[0]);
                    }
                },
            });
        }

        $(this).parent().parent().children("p").attr("contenteditable", false);
        $(this).parent().children(".edit, .remove").removeClass("hidden");
        $(this).parent().children(".ok, .cancel").addClass("hidden");
        editing = false;
    });

    $("form").submit(function (e) {
        var form = this;

        e.preventDefault();

        $.ajax({
            type: $(form).attr("method"),
            dataType: "json",
            url: $(form).attr("action"),
            data: $(form).serialize(),
            success: function (response) {
                if (response.status) {
                    location.reload();

                    return true;
                }

                $(form).children("ul").remove();
                $(form).append("<ul></ul>");
                $(response.errors).each(function (key, value) {
                    $(form).children("ul").append("<li>" + value + "</li>")
                });
            },
        });

        return false;
    });

});
$(document).ready(function () {

    $(document).on("click", ".category", function (e) {

        e.preventDefault();
        let idCategory = $(this).attr("data-id");
        let parent = $(".product-grid-view")
        $.ajax({

            //urlde:-contoleri ve actionu yaziriq ve skip-adli varebla deyer gonderik,ordan gebul edib skip elesin deye
            url: `shop/GetProductsByCategory?id=${idCategory}`,
            //type:-datani gotururuk deye type=get
            type: "Get",

            //succsesden sonra hansi function islesin
            success: function (res) {
                console.log(parent)
                $(parent).append(res)
            }
        })



    })
})
//SEARCH WITH li

$(document).on("keyup", ".input-field", function () {
    $("#search-list li").slice(1).remove();
    let value = $(".input-field").val();

    $.ajax({

        url: `shop/search?searchText=${value}`,

        type: "Get",

        success: function (res) {

            $("#search-list").append(res);
        }



    })


})



//MAIN SEARCH

$(document).on("submit", ".hm-searchbox", function (e) {
    e.preventDefault();
    let value = $(".input-search").val();
    let url = `/shop/mainsearch?searchText=${value}`;

    window.location.assign(url);


})


//---------------ADD-PRODUCTS TO BASKET WITH REQUEST AJAX(Ajaxla productu add ediremki refresh olmasin)---------------

$(document).on("click", ".add-basket", function () {

    let productId = $(this).parent().attr("data-id")
    let parent =$(".minicart-list")
    let data = { id: productId }
    $.ajax({

        url: "home/addBasket",
        type: "Post",
        data: data,
        success: function (res) {
            $(".quantity").text(res)

        }
    })
    return false;


})


//---------------DELETE PRODUCT FROM BASKET WITH AJAX------------------------

$(document).on("click", ".delete-basketProduct", function (e) {      

    e.preventDefault();

    let deletProduct = $(this).parent().parent(); 

    let productId = $(this).attr("data-id")                         

    let sum = 0;

    let grandTotal = $(".total-product").children().eq(0);        


    $.ajax({

        url: `basket/DeleteProductFromBasket?id=${productId}`,       

        type: "Post",
        success: function (res) {
            res--
            $(".quantity").text(res)
            $(deletProduct).remove();                             
            //swal("Product deleted to basket", "", "success");

            //for (const product of $(".table-product").children()) {     
            //    let total = parseFloat($(product).children().eq(6).text())  
            //    sum += total    

            //}
            //$(grandTotal).text(sum);   

            //if ($(".table-product").children().length == 0) {     
            //    $("table").addClass("d-none");                     
            //    $(".total-product").addClass("d-none");            
            //    $(".alert-product").removeClass("d-none");           


            }



        }
    })
    return false;


})




//Decrease Product from Basket (product sayini azaltmaq basketde) 
$(document).on("click", ".minus", function () {


    let productId = $(this).parent().parent().attr("data-id");   

    let input = $(this).next()     

    let count = parseInt($(input).val()) - 1;   


    let nativePrice = parseFloat($(this).parent().prev().text())   

    let total = $(this).parent().next().children().eq(0);          

    let sum = 0;

    let grandTotal = $(".total-product").children().eq(0);          


    if (count > 0) {         
        $(input).val(count);

        $.ajax({

            url: `card/DecreaseCountProductFromBasket?id=${productId}`,      

            type: "Post",

            success: function (res) {

                let countProduct = res;              
                let subtotal = nativePrice * countProduct  
                total.text(subtotal + ",00")             
                for (const product of $(".table-product").children()) {    

                    let total = parseFloat($(product).children().eq(6).text())  
                    sum += total   

                }
                $(grandTotal).text(sum + ",00");   



            }
        })
    }







})



//Increase Product from Basket (product sayini coxaltmaq basketde) 
$(document).on("click", ".plus", function () {

    let productId = $(this).parent().parent().attr("data-id");  

    let input = $(this).prev()  

    let count = parseInt($(input).val()) + 1;  

    $(input).val(count);  

    let nativePrice = parseFloat($(this).parent().prev().text()) 


    let total = $(this).parent().next().children().eq(0);              

    let sum = 0;

    let grandTotal = $(".total-product").children().eq(0);      


    $.ajax({

        url: `card/IncreaseCountProductFromBasket?id=${productId}`,         

        type: "Post",
        success: function (res) {

            let countProduct = res;      
            let subtotal = nativePrice * countProduct   
            total.text(subtotal + ",00")                   
            for (const product of $(".table-product").children()) {     

                let total = parseFloat($(product).children().eq(6).text())   

                sum += total    

            }
            $(grandTotal).text(sum + ",00");   


        }
    })
    return false;

})


var scrollPosition = localStorage.getItem('scrollPosition');
if (scrollPosition !== null) {
    $(window).scrollTop(scrollPosition);
    localStorage.removeItem('scrollPosition');
}

$("button").on("click", function () {
    
});


var promise = new Promise((resolve, reject) => {
    setTimeout(resolve, 2000)
})


//$('.tb-data tr').click(function () {
//    var text = $(this).find('td:first').text();
//    $('.bd-example-modal-lg').modal('show')
//    let newElement = $(`<h1>${text}</h1>`)

   
//    //console.log($('.modal-body .ac-hide'))
//});
var obj = {
    id: 1,
    name: 'tut ut'
}


$(document).ready(function () {
    
});
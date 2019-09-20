 // scroll from top js 

    $(document).ready(function(){
    $(this).scrollTop(0);

  
    // //$(".boxer").boxer();
    // $(".boxer").boxer({
    //   fixed: true
    // });
    
    if($('.galleryBox').length){
     jQuery('ul.galleryBox > li').hoverdir();
    }

 // For Front-Slider 
if ($(window).width() > 1200) {

  function setHeight() {
    windowHeight = $(window).innerHeight();
    $('#bootstrap-touch-slider').css('min-height', windowHeight);
  };
  setHeight();

}

  function setHeight() {
    windowHeight = $(window).innerHeight();
    $('.modal-content').css('min-height', windowHeight);
  };
  setHeight();


 // sticky header 

$(window).scroll(function(){
    if ($(window).scrollTop() > 1) {
       $('#sticky-wrap').addClass('fixed-header');
    }
    else {
       $('#sticky-wrap').removeClass('fixed-header');
    }

    // ===== Scroll to Top ==== 
    if ($(this).scrollTop() >= 50) {        // If page is scrolled more than 50px
        $('#return-to-top').fadeIn(200);    // Fade in the arrow
    } else {
        $('#return-to-top').fadeOut(200);   // Else fade out the arrow
    }

    var scrollDistance = $(window).scrollTop();

    $('section').each(function(i) {
      if ($(this).position().top <= scrollDistance + 1) {
        $('.header-list-right li a').removeClass('active');
        $('.header-list-right li a').eq(i).addClass('active');
      }
  });
});



$('#return-to-top').click(function() {
    $('body,html').animate({
        scrollTop : 0                       // Scroll to top of body
    }, 1000);
});

$('li a[href*="#"]').on('click', function (e) {
	e.preventDefault();

  $('.header-list-right li a').removeClass('active');
  $(this).addClass('active');

	$('html, body').animate({
		scrollTop: $($(this).attr('href')).offset().top
	}, 500, 'linear');
});


  // Toggle Menu 


  $('.menu-open').click(function(e){
    e.stopPropagation();
    $('.mobile-left-menu').toggleClass('open');
    $('body').toggleClass("fixedPosition");

  });
  
  $('section,footer').click(function(e){
   $('.mobile-left-menu').removeClass('open');
   $('body').removeClass("fixedPosition");
    $('.home').removeClass('show-list');
    $('.about').removeClass('show-list');

  });



    $('.mobile-left-menu li.close-menu').click(function(e) {

        $('.mobile-left-menu').toggleClass('open' );
        $('body').removeClass("fixedPosition");
        $('.home').removeClass('show-list');
        $('.about').removeClass('show-list');

      });




  
   $('.other-theme').click(function(e){
    $('.home').toggleClass('show-list');
    $('.menu-right-arrow').toggleClass('down');
  });
     $('.aboutus-list').click(function(e){
    $('.about').toggleClass('show-list');
    $('.menu-right-arrow1').toggleClass('down');
  });

 // For Id Called To The Data-Target 



  if (window.location.hash)
    scroll(0,0);
// takes care of some browsers issue
setTimeout(function(){scroll(0,0);},1);

$(function(){

// if we have anchor on the url (calling from other page)
if(window.location.hash){
    // smooth scroll to the anchor id
    $('html,body').animate({
        scrollTop:$(window.location.hash).offset().top - $('#sticky-wrap').outerHeight()
        },1000,'swing');
    }
});



 // For Search Box 

   $('.search_icon').click(function(e){
    e.stopPropagation();
    $('.search').toggleClass('active-search');
  });
  

   $('.close-x').click(function(e){
    e.stopPropagation();
    $('.search').removeClass('active-search');
  });


 // For Login-Signup Box 


  $('.toggle').on('click', function() {
  $('.pop_up').stop().addClass('active');
});

$('.close').on('click', function() {
  $('.pop_up').stop().removeClass('active');
});
$('.sign_up_cls').on('click',function(){
  $('.pop_up').addClass('active');
});

$('#login-model').on('hidden.bs.modal', function () {
  $('.pop_up').removeClass('active');
})

/* Owl Slider */

$('.owl-carousel').owlCarousel({
        loop:true,
        margin:10,
        responsiveClass:true,
        responsive:{
            0:{
                items:1,
                nav:true
            },
            481:{
                items:2,
                nav:false
            },
            1000:{
                items:3,
                nav:true,
                loop:false
            }
        }
    })

    
// /* Boxer Slider */
// $(".boxer").boxer();

});

/* Counter */    

(function($) {
    "use strict";
    function count($this){
    var current = parseInt($this.html(), 10);
    current = current + 1; /* Where 50 is increment */  
    $this.html(++current);
      if(current > $this.data('count')){
        $this.html($this.data('count'));
      } else {    
        setTimeout(function(){count($this)}, 50);
      }
    }         
    $(".stat-count").each(function() {
      $(this).data('count', parseInt($(this).html(), 10));
      $(this).html('0');
      count($(this));
    });
   })(jQuery);

   
$(".letsTalkBtn").click(function(){
    $(".letsTalk").css("height","300");
});


(function conveyorLoop() {
    $('.facebook-section').animate({
        'background-position-x': '+=1000'
    }, 8000, 'linear', conveyorLoop);
}());


// $('.projects').on('click',function(e){
//     e.preventDefault();
//     $('html,body').animate({
//         scrollTop:$($(this).attr('href')).offset().top - $('#sticky-wrap').outerHeight()
//     },1000,'swing');
// });

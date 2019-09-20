jQuery(function($) {
	//Initiat WOW JS
	new WOW().init();

	$.getJSON("./assets/json/slider.json", function( data ) {
		var nav = '';
		var images = '';

		$.each( data, function( key, val ) {
			var active = "";

            if (key == 0) {
                active = "active";
            }

			nav +=`<li data-target="#bootstrap-touch-slider" data-slide-to="${key}" class="${active}"></li>`;

			images +=`<div class="item ${active}">`+
						`<img src="./assets/img/slider/${ val.image }" alt="${ val.heading }" class="img-responsive">`+
						`<div class="slide-text slide_style_center">`+
							`<div class="sliderDetails">`+
								`<div class="briefInfo wow fadeInDown">`+
									`<h3 class="font-LB">${ val.heading }</h3>`+
									`<p>${ val.description }</p>`+
								`</div>`+
							`</div>`+
						`</div>`+
					`</div>`;
		});
	
		$('.carousel-indicators').html(nav);
		$('.carousel-inner.cont-slider').html(images);
	});

	$.getJSON("./assets/json/gallery.json", function( data ) {
		var images = '';

		$.each( data, function( key, val ) {
			images += `<a href="http://unitegallery.net">`+
							`<img alt="${val.heading}" src="./assets/img/gallery/${val.image}" data-image="./assets/img/gallery/${val.image}" data-description="${val.description}" style="display:none">`+
						`</a>`
		});
	
		$('#gallery-content').html(images);
		$("#gallery-content").unitegallery();
	});
});
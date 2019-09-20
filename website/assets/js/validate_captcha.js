    /******************************** Form Validation Section ********************************/
    jQuery(document).ready(function(){ 
    $('#submit').click(function() {
        $('#fname').css('border-color','#ccc');
        $('#lname').css('border-color','#ccc');
        $("#email").css('border-color','#ccc');
        $('#subject').css('border-color','#ccc');
        $('#message').css('border-color','#ccc');

        if($.trim($("#fname").val())== '')
        {
          $('#fname').select();
          $('#fname').focus();
          $('#fname').css('border-color','#F51313');
          return false;
        }

        else if($.trim($("#lname").val())== '')
        {
          $('#lname').select();
          $('#lname').focus();
          $('#lname').css('border-color','#F51313');
          return false;
        }
        else if($.trim($("#email").val()) == '')
        {
          $('#email').select();
          $('#email').focus();
          $('#email').css('border-color','#F51313');
          return false;

        }
        else if($.trim($("#subject").val()) == '')
        {
          $('#subject').select();
          $('#subject').focus();
          $('#subject').css('border-color','#F51313');        
          return false;

        }
        else if($.trim($("#message").val()) == '')
        {
          $('#message').select();
          $('#message').focus();
          $('#message').css('border-color','#F51313');
          return false;
        }
        else
        {
          return true;
        }

  });
});

/******************************** Captcha validation & Form Submit Section ********************************/

  var form = $('#form');
  var success_status = $('.form_status');
  var resultcaptchacontact;
  function submitContact()
  { 
    if($('#recaptcha-anchor').attr('aria-checked')=='true')
    {
       form.submit();
    }
  }
  form.submit(function(event)
  {
    event.preventDefault();
    var form_status = $('<div class="form_status"></div>');
    var $form = $(this);
    var captcha = grecaptcha.getResponse(resultcaptchacontact);

    if(captcha.length == 0)
    {
      $('#contacterror').html('<div class="w3-text w3-bold" style="font-size:16px; color:#F00; float:left;">Please verify that you are a human</div>');
    }
    else
    {
      $('#contacterror').html('');
      $.ajax({
      type: 'POST',
      url: "./contact_action.php",
      data:{
        firstname: $form.find( "input[name='firstname']" ).val(),
        lastname: $form.find( "input[name='lastname']" ).val(),
        email: $form.find( "input[name='email']" ).val(),
        Subject: $form.find( "input[name='Subject']" ).val(),
        message: $form.find( "textarea[name='message']" ).val(),
        
      },
      beforeSend: function(){
         success_status.append( form_status.html('<p style="font-weight:bold;color:white;"><i class="fa fa-spinner w3-text-green w3-spin" style="font-size:50px"></i> Email is sending...</p>').fadeIn() );
      }
    }).done(function(data){
       form_status.html('<i class="fa fa-check fa-4x w3-text-green" aria-hidden ="true"></i> <span style="color:green;text-align:justify" class="text-success w3-animate-fading">Your message has been sent!</span>').delay(20000).fadeOut();
    });

      //clear values when submit
      document.getElementById("fname").value = "";
      document.getElementById("lname").value = "";
      document.getElementById("email").value = "";
      document.getElementById("subject").value = "";
      document.getElementById("message").value = "";
      grecaptcha.reset(resultcaptchacontact);
    }
  });   

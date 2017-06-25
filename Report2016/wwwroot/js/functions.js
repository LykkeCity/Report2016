var deviceAgent = navigator.userAgent.toLowerCase(),
  isMobile = deviceAgent.match(/(iphone|ipod|ipad)/),
  is_touch_device = ("ontouchstart" in window) || window.DocumentTouch && document instanceof DocumentTouch;

function initEventsOnResize() {
  $(window).resize(function() {
    var  wH = $(window).height();

    $('body').css({
      paddingBottom: $('footer').outerHeight()
    });

    $('.valign').css({
      minHeight: wH - ($('.r_header').outerHeight() + $('.footer').outerHeight()) - 50
    })
  }).trigger('resize');
}

function initEventsOnClick() {
  // Tel
  if (!isMobile) {
    $('body').on('click', 'a[href^="tel:"]', function() {
      $(this).attr('href',
        $(this).attr('href').replace(/^tel:/, 'callto:'));
    });
  }
}

function initEventsOnLoad() {
  $(window).on('load', function() {
    $('body').addClass('loaded');

    setTimeout(function() {
      $('.r_candle').each(function() {
        $(this).css({
          height: parseInt($(this).data('height')) + '%'
        })
      });
    }, 100);
  })
}

function textareaCounter() {
  $("textarea#comment").keyup(function(){
    console.log('keyup', $(this).attr('maxlength'))
    var maxlength = $(this).attr('maxlength');
    $("._counter").text(maxlength - $(this).val().length);
  });
}


$(document).ready(function() {
  initEventsOnResize();
  initEventsOnClick();
  initEventsOnLoad();
  textareaCounter();
});
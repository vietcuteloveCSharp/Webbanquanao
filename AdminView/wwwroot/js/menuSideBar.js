if (typeof window.isMenuInitialized === 'undefined') {
    window.isMenuInitialized = false;
}

$(document).ready(function () {
    if (!window.isMenuInitialized) {
        $('#left-menu .nav-link').off('click').on('click', function (e) {
            e.preventDefault();

            const url = $(this).data('url');
            const rightContent = $('#right-content');

            rightContent.html('<div id="loading">Đang tải...</div>');

            $.get(url, function (data) {
                rightContent.html(data);
            });
        });
        window.isMenuInitialized = true;
    }
});

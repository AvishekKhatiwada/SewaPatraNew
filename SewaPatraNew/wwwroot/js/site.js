// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

setTimeout(function () {
    var toast = document.getElementById('toastMessage');
    if (toast) {
        toast.style.opacity = '0';
        setTimeout(() => toast.remove(), 500);
    }
}, 2000);

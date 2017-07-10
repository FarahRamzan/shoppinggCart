$(document).ready(function () {

//Client

var hash = $.trim( window.location.hash );
if (hash) $('.nav-tabs a[href$="'+hash+'"]').trigger('click');

$(window).on('hashchange', function() {
	var hash = $.trim( window.location.hash );
	if (hash) $('.nav-tabs a[href$="'+hash+'"]').trigger('click');
});

var selectAns = $("#new_loc").val();
$("#loc_txt option[value='" + selectAns + "']").prop('selected', true);

var selectAns2 = $("#new_cat").val();
$("#cat_txt option[value='" + selectAns2 + "']").prop('selected', true);

var selectAns3 = $("#new_sort_type").val();
$("#sort_type option[value='" + selectAns3 + "']").prop('selected', true);

$("#iconEye").show();
$("#iconEyeR").show();

$("#del-act").click(function () {
    var id = $("#user_id").val();
    $.getJSON("/Account/DeleteAccount?id=" + id, function (result) {
        if (result.status == false) {
            showNotification("Error Occurred", "danger");
        }
        else
        {
            showNotification("Account Deleted Successfully", "success");
            goToHome().delay(5000);
        }
    });
});

function goToHome()
{
    window.location.href = '/Home/Index';
}

$("#bookmarkStar").click(function (e) {
    var adId = $("#ad_id").val();
    $.getJSON("/Ads/IsBookmarkedAlready?id=" + adId, function (result) {
        if (result.status == false) {
            $("#modal_alert").modal();
        }
        else
        {
            showNotification("Ad has been bookmarked successfully", "success");
        }
    });
});

$("#ad_edit_btn").click(function (e) {
    e.preventDefault(e);

    var ans = true;

    if (!isTitleOk())
        ans = false;

    if (!isDescOk())
        ans = false;

    if (!isPriceOk())
        ans = false;

    if (ans) {
        showNotification("Ad has been updated successfully.", "success");
        $("#edit_ad_form").submit();
    }
});

$("#ad_submit_btn").click(function (e) {
    e.preventDefault(e);

    var ans = true;

    if (!isTitleOk())
        ans = false;

    if (!isDescOk())
        ans = false;

    if (!isPriceOk())
        ans = false;

    if (ans)
    {
        showNotification("Ad has been submitted Successfully.", "success");
        $("#submit_ad_form").submit().delay(3000);
    }
});

$("#ad_title").focusout(function (e) {
    isTitleOk();
});

function isTitleOk() {
    $('#ad_title').popover({ title: "Invalid Input", content: "Default.", placement: "left", animation: true, trigger: "manual" });

    if ($("#ad_title").val() == "") {
        $('#ad_title').data('bs.popover').options.content = "Ad Title is required. It cannot be empty.";
        $('#ad_title').popover("show");
        return false;
    }
    else {
        $("#ad_title").popover("hide");
        return true;
    }
}

$("#ad_desc").focusout(function (e) {
    isDescOk();
});

function isDescOk() {
    $('#ad_desc').popover({ title: "Invalid Input", content: "Default.", placement: "right", animation: true, trigger: "manual" });

    if ($("#ad_desc").val() == "") {
        $('#ad_desc').data('bs.popover').options.content = "Ad Description is required. It cannot be empty.";
        $('#ad_desc').popover("show");
        return false;
    }
    else {
        $("#ad_desc").popover("hide");
        return true;
    }
}

$("#ad_price").focusout(function (e) {
    isPriceOk();
});

function isPriceOk() {
    $('#ad_price').popover({ title: "Invalid Input", content: "Default.", placement: "left", animation: true, trigger: "manual" });

    if ($("#ad_price").val() == "") {
        $('#ad_price').data('bs.popover').options.content = "Ad Price is required. It cannot be empty.";
        $('#ad_price').popover("show");
        return false;
    }
    else {
        $("#ad_price").popover("hide");
        return true;
    }
}

$("#login_btn").click(function (e) {
    e.preventDefault(e);

    var ans = true;

    if (!isEmailOk())
        ans = false;

    if (!isPassOk())
        ans = false;

    if (ans)
    {
        var email = $("#login_email").val();
        var password = $("#login_pass").val();
        $.getJSON("/Account/IsCredentialsCorrect?email=" + email + "&password=" + password, function (result) {
            if (result.status == false)
            {
                $("#modal_alert").modal();
            }
            else
            {
                $("#login_form").submit();
            }
        });
    }
});

$("#reg_btn").click(function (e) {
    e.preventDefault(e);
	
	var ans = true;
	
	if(!isEmailOk())
		ans = false;
		
	if(!isPassOk())
	    ans = false;

	if (!isNumbOk())
	    ans = false;

	if (!isPassROk())
	    ans = false;

	if (!isNameOk())
	    ans = false;
	
	if(ans)
	{
	    var email = $("#login_email").val();
	    $.getJSON("/Account/IsAlreadyExists?email=" + email, function (result) {
	        if (result.status == true)
	        {
	            $("#modal_alert").modal();
	        }
	        else
	        {
	            showNotification("User has been added Successfully.", "success");
	            $("#reg_form").submit().delay(3000);
	        }
	    });
	}
});

$("#login_email").focusout(function(e) {
	isEmailOk();
});

function isEmailOk() {
	$('#login_email').popover({title: "Invalid Input", content: "Default.", placement: "right",animation:true,trigger:"manual"});
	
	if($("#login_email").val() == "")
	{
		$('#login_email').data('bs.popover').options.content = "Email is required. It cannot be empty.";
		$('#login_email').popover("show");
		return false;
	}
    else if(!isValidEmailAddress($("#login_email").val()) ) 
	{
		$('#login_email').data('bs.popover').options.content = "Invalid format of email.";
		$('#login_email').popover("show");
		return false;
	}
	else
	{
		$("#login_email").popover("hide");	
		return true;
	}	
}

function isValidEmailAddress(emailAddress) {
    var pattern = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return pattern.test(emailAddress);
};

$("#login_pass").focusout(function(e) {
	isPassOk();
});

function isPassOk() {
	$('#login_pass').popover({title: "Invalid Input", content: "Default.", placement: "left", animation:true, trigger:"manual"});
	
	if($("#login_pass").val() == "")
	{	
		$('#login_pass').data('bs.popover').options.content = "Password is required. It cannot be empty.";
		$('#login_pass').popover("show");
		return false;
	}
    else if(!isValidPass( $("#login_pass").val() ) ) 
	{
		$('#login_pass').data('bs.popover').options.content = "Invalid format of password. It must have at least one number, one lowercase and one uppercase letter. It must be at least eight characters long.";
		$('#login_pass').popover("show");
		return false;
	}
	else
	{	
		$("#login_pass").popover("hide");
		return true;
	}
}

function isValidPass(str) {
    var re = /(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}/;
    return re.test(str);
};

$("#reg_name").focusout(function(e) {
	isNameOk();
});

function isNameOk() {
	$('#reg_name').popover({title: "Invalid Input", content: "Default.", placement: "left", animation:true, trigger:"manual"});
	
	if($("#reg_name").val() == "")
	{	
		$('#reg_name').data('bs.popover').options.content = "Name is required. It cannot be empty.";
		$('#reg_name').popover("show");
		return false;
	}
	else
	{	
		$("#reg_name").popover("hide");
		return true;
	}
}

$("#passR").focusout(function(e) {
	isPassROk();
});

function isPassROk() {
	$('#passR').popover({title: "Invalid Input", content: "Default.", placement: "right", animation:true, trigger:"manual"});
	
	if($("#passR").val() == "")
	{	
		$('#passR').data('bs.popover').options.content = "Password is required. It cannot be empty.";
		$('#passR').popover("show");
		return false;
	}
	else if($("#passR").val() != $("#login_pass").val())
	{
		$('#passR').data('bs.popover').options.content = "Password does not match with above password.";
		$('#passR').popover("show");
		return false;
	}
	else
	{	
		$("#passR").popover("hide");
		return true;
	}
}

$("#reg_num").focusout(function(e) {
	isNumbOk();
});

function isNumbOk() {
	$('#reg_num').popover({title: "Invalid Input", content: "Default.", placement: "right", animation:true, trigger:"manual"});
	
	if($("#reg_num").val() == "")
	{	
		$('#reg_num').data('bs.popover').options.content = "Phone No. is required. It cannot be empty.";
		$('#reg_num').popover("show");
		return false;
	}
	else
	{	
		$("#reg_num").popover("hide");
		return true;
	}
}
	
$.fn.toggleRemoveBtns = function()
{
    if ($('#upload1').attr('src') != "/Content/img/upload_img.jpg")
		$('#remove-img1').show();
	else
		$('#remove-img1').hide();	
	
    if ($('#upload2').attr('src') != "/Content/img/upload_img.jpg")
		$('#remove-img2').show();
	else
		$('#remove-img2').hide();
	
    if ($('#upload3').attr('src') != "/Content/img/upload_img.jpg")
		$('#remove-img3').show();
	else
		$('#remove-img3').hide();
	
    if ($('#upload4').attr('src') != "/Content/img/upload_img.jpg")
		$('#remove-img4').show();
	else
		$('#remove-img4').hide();
}

$.fn.toggleRemoveBtns();

function readURL(input, img) {
    if (input.files && input.files[0])
    {
        var file = input.files[0];
        var sFileName = file.name;
        var sFileExtension = sFileName.split('.')[sFileName.split('.').length - 1].toLowerCase();
        var iFileSize = file.size;
        var iConvert = (file.size / 10485760);

        if (!(sFileExtension === "png" || sFileExtension === "jpg" || sFileExtension === "jpeg") || iFileSize > 10485760) {
            txt = "Invalid File Uploaded!<br />File type : " + sFileExtension + "<br />";
            txt += "Size: " + iConvert + " MB<br />";
            txt += "Please make sure your file is in png, jpg or jpeg format and less than 10 MB.";
            showNotification(txt, "danger");
            return false;
        }
        else
        {
            var reader = new FileReader();

            reader.onload = function (e) {
                img.attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
            return true;
        }
    }
}

$(".fileInput").change(function () {

    var result = readURL(this, $(this).parent().children([0]).children([0]));
    if(result == false)
    {
        $(this).val(null);
    }
    else
    {
        $(this).parent().children([1]).show(250);
        $(this).hide();
    }
});
	
function removePhoto()
{
	var id = "#" + $(this).parent().children([0]).children([0]).attr('id');
	$(this).parent().children().next().next().attr('val',null);
	$(id).attr('src', '/Content/img/upload_img.jpg');
	$(this).hide(250);
}

$('.remove-img').on('click',removePhoto);
	
function removeMyAd()
{
    var id = "" + $(this).parent().parent().parent().attr('id');
    var adId = "#" + id;
		
	$.getJSON("/Ads/RemoveAdEntry?idStr=" + id, function (result) {
	    if (result.status == true) {
	        $(adId).hide('slow', function () {
	            $(adId).remove();
	            if ($("#my-ads-rows div").children().length == 1)
	                $("#my-def-ad-row").show(1000);
	        });
	        showNotification("Ad has been removed successfully.", "success");
	    }
	});
}

$('.my-remove-ad').on('click',removeMyAd);
	
function removeAd()
{
    var id = "" + $(this).parent().parent().parent().attr('id');
    var adId = "#" + id;

    $.getJSON("/Ads/RemoveAdBookmarkEntry?idStr=" + id, function (result) {
        if (result.status == true) {
            $(adId).hide('slow', function () {
                $(adId).remove();
                if ($("#ads-rows div").children().length == 1)
                    $("#def-ad-row").show(1000);
            });
            showNotification("Ad's bookmark has been removed successfully.", "success");
        }
    });
}

$('.remove-ad').on('click',removeAd);

function showNotification(msg, mode)
{
	$.notify({
		// options
		icon: 'glyphicon glyphicon-warning-sign',
		title: '',
		message: msg
	},{
		// settings
	    type: mode,
		allow_dismiss: true,
		newest_on_top: true,
		placement: {
			from: "top",
			align: "right"
		},
		offset: 70,
		spacing: 10,
		z_index: 1031,
		delay: 5000,
		timer: 1000,
		url_target: '_blank',
		mouse_over: 'pause',
		animate: {
			enter: 'animated fadeInDown',
			exit: 'animated fadeOutUp'
		},
		onShow: null,
		onShown: null,
		onClose: null,
		onClosed: null,
		icon_type: 'class',
		template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
			'<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
			'<span data-notify="icon"></span> ' +
			'<span data-notify="title">{1}</span> ' +
			'<span data-notify="message">{2}</span>' +
			'<div class="progress" data-notify="progressbar">' +
				'<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
			'</div>' +
			'<a href="{3}" target="{4}" data-notify="url"></a>' +
		'</div>' 
	});
}

$("#range1").on('change',function(){
	$("#range2").attr("min",$("#range1").val());
});

$("#range2").change(function(){
	$("#range1").attr("max",$("#range2").val());
});

$("#login_pass").on("keyup",function(){
    if($(this).val())
        $("#iconEye").show();
    else
        $("#iconEye").hide();
});

$("#iconEye").mousedown(function(){
    $("#login_pass").attr('type', 'text');
	}).mouseup(function(){
	    $("#login_pass").attr('type', 'password');
	}).mouseout(function(){
	    $("#login_pass").attr('type', 'password');
});

$("#passR").on("keyup",function(){
    if($(this).val())
        $("#iconEyeR").show();
    else
        $("#iconEyeR").hide();
});

$("#iconEyeR").mousedown(function(){
		$("#passR").attr('type','text');
	}).mouseup(function(){
		$("#passR").attr('type','password');
	}).mouseout(function(){
		$("#passR").attr('type','password');
	});

//Admin

$("#login_admin_btn").click(function (e) {
    e.preventDefault(e);

    var ans = true;

    if (!isEmailOk())
        ans = false;

    if (!isPassOk())
        ans = false;

    if (ans) {
        var email = $("#login_email").val();
        var password = $("#login_pass").val();
        $.getJSON("/Admin/IsAdminCorrect?email=" + email + "&password=" + password, function (result) {
            if (result.status == false) {
                $("#modal_alert").modal();
            }
            else {
                $("#login_admin_form").submit();
            }
        });
    }
});

$("#admin_upd_pro_btn").click(function (e) {
    e.preventDefault(e);

    var ans = true;

    if (!isPassOk())
        ans = false;

    if (!isPassROk())
        ans = false;

    if (!isNameOk())
        ans = false;

    if (ans) {
        showNotification("Your Profile has been Updated Successfully.", "success");
        $("#admin_profile_update_form").submit().delay(3000);
    }
});

$("#upd_pro_btn").click(function (e)
{
    e.preventDefault(e);

    var ans = true;

    if (!isPassOk())
        ans = false;

    if (!isNumbOk())
        ans = false;

    if (!isPassROk())
        ans = false;

    if (!isNameOk())
        ans = false;

    if (ans)
    {
        showNotification("User Profile has been Updated Successfully.", "success");
        $("#profile_update_form").submit().delay(3000);
    }
});

$('.remove-user').on('click', removeUser);

function removeUser() {
    var id = "" + $(this).parent().parent().attr('id');
    var adId = "#" + id;

    $.getJSON("/Admin/RemoveUserEntry?idStr=" + id, function (result) {
        if (result.status == true) {
            $(adId).hide('slow', function () {
                $(adId).remove();
            });
            showNotification("User has been removed successfully.", "success");
        }
    });
}


$('.remove-ad-by-admin').on('click', removeAdByAdmin);

function removeAdByAdmin() {
    var id = "" + $(this).parent().parent().attr('id');
    var adId = "#" + id;

    $.getJSON("/Admin/RemoveAdEntry?idStr=" + id, function (result) {
        if (result.status == true) {
            $(adId).hide('slow', function () {
                $(adId).remove();
            });
            showNotification("Ad has been removed successfully.", "success");
        }
    });
}

});
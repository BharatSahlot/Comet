mergeInto(LibraryManager.library, {

  InitSDK: function (version, objectName) {
    Crazygames.init({
      version: Pointer_stringify(version),
      crazySDKObjectName: Pointer_stringify(objectName),
      sdkType: "unity5.6",
    });
  },

  GetScreenshotSDK: function(img, size) {
    var binary = '';
    for (var i = 0; i < size; i++)
        binary += String.fromCharCode(HEAPU8[img + i]);
    var dataUrl = 'data:image/png;base64,' + btoa(binary);
    Crazygames.screenshotReceived(dataUrl);
  },

  RequestAdSDK: function (adType) {
    Crazygames.requestAd(Pointer_stringify(adType));
  },

  HappyTimeSDK: function () {
    Crazygames.happytime();
  },

  GameplayStartSDK: function () {
    Crazygames.gameplayStart();
  },

  GameplayStopSDK: function () {
    Crazygames.gameplayStop();
  },

  RequestInviteUrlSDK: function (url) {
    Crazygames.requestInviteUrl(Pointer_stringify(url));
  },

  GetUrlParametersSDK: function () {
    const urlParameters = window.location.search;
    var bufferSize = lengthBytesUTF8(urlParameters) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(urlParameters, buffer, bufferSize);
    return buffer;
  },

  CopyToClipboardSDK: function (text) {
   const elem = document.createElement('textarea');
   elem.value = Pointer_stringify(text);
   document.body.appendChild(elem);
   elem.select();
   elem.setSelectionRange(0, 99999);
   document.execCommand('copy');
   document.body.removeChild(elem);
  },

  RequestBannersSDK: function(bannersJSON) {
    const banners = JSON.parse(Pointer_stringify(bannersJSON));
    Crazygames.requestBanners(banners);
  },
});

// $('#btn_ok_fileUploadModalDialog').click(function () {
$(document).on("click", '#btn_ok_fileUploadModalDialog', function () {
    // ファイル読み込みに参考にしたサイト
    // https://kurage.ready.jp/jhp_g/html5/localF.html
    try {
        var file = document.querySelector('#fileUploadModalForm');
        var fileList = file.files;
        var render = new FileReader();
        render.readAsText(fileList[0]);
        render.onload = function () {
            document.querySelector(".text-code").innerHTML = render.result;
            $("#fileName").text(fileList[0].name);
            $('#fileUploadModalDialog').modal('hide');
            SORAMAME_BLOCK.setSerializeBlock();
        }
    } catch (e) {
        if (e instanceof TypeError) {
            // $(".invalid-feedback").show();  
            window.alert("ファイルを選択してください");
        }
    }
});

async function save() {
    // ファイル保存に参考にしたサイト
    // https://www.nightswinger.dev/2021/12/save-text-file-with-showsavefilepicker/
    const opts = {
        suggestedName: 'Form',
        types: [{
            description: 'Block code file',
            accept: { 'application/block': ['.blk'] },
        }],
    };
    const handle = await window.showSaveFilePicker(opts);
    const writable = await handle.createWritable();
    var output = document.querySelector(".text-code").innerHTML.replace(/\s{4}/g, "");
    await writable.write(output);
    await writable.close();
    $("#fileName").text(handle.name);
}

async function window_load(fileName, code) {
    console.log("Loading...");
    document.querySelector(".text-code").innerHTML = code;
    $("#fileName").text(fileName);
    SORAMAME_BLOCK.setSerializeBlock();
    console.log("Loaded.");
}


$(function () {
    $('#Cnpj').mask('99.999.999/9999-99');
    $('#Telefone1').mask('(99) 9999-99999');
    $('#Telefone2').mask('(99) 9999-99999');
});



function mensagemOk(idEmpresa) {
    $(function () {
        $("#modalPopup").html(' \
                <p><span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span> \
                    Atualização realizada com sucesso! \
                </p>'
    ).dialog({
        width: 400,
        modal: true,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            },
            "Nova Unidade":
                function () {
                    location.href = '/Filial/Cadastrar/?idEmpresa='+idEmpresa;
                }
        }
    });
    });
}
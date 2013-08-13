
$(function () {

    // IE10 e outros navegadores
    if (getInternetExplorerVersion() >= 10 || getInternetExplorerVersion() == -1) {
        $('#fileUpload').fileupload({
            dataType: 'json',
            done: function (e, data) {
                $('#fotoFuncionario').attr('src', data.result[0].url);
                $('#Foto').val(data.result[0].name);
            }
        });
        $('#inputFile').hide();
        $('#fotoFuncionario').click(function () {
            $('#inputFile').click();
        });
    }
    else {// < IE10
        $("#fileUpload").after('<iframe id="uploader_iframe" name="uploader_iframe" style="display: none;"></iframe>');
        $("#fileUpload").attr('target', 'uploader_iframe');
        $("#inputFile").change(function () {
            $("#fileUpload").submit();  // Submits the form on change event, you consider this code as the start point of your request (to show loader)
            $("#uploader_iframe").unbind().load(function () {  // This block of code will execute when the response is sent from the server.
                result = JSON.parse($(this).contents().text());  // Content of the iframe will be the response sent from the server. Parse the response as the JSON object
                $('#fotoFuncionario').attr('src', result[0].url);
                $('#Foto').val(result[0].name);
            });
        });
    }
    
    $('#Nome').keyup(function () {
        $('#spanNome').html($('#Nome').val());
    });

    $('#Nome').blur(function () {
        $('#spanNome').html($('#Nome').val());
    });

    $('#spanNome').html($('#Nome').val());
});

function selecionarGestor(idFuncionario, nome) {
    $('#NomeGestor').val(nome);
    $('#IdGestor').val(idFuncionario);
    $('#FuncionarioLista').dialog("destroy");
    $('#FuncionarioLista').hide();
}

function abrirModalPesquisarGestor() {
    $('#modalPopup').load('/Funcionario/PopupListar', function () {
        listarFuncionarios();

        $('#FuncionarioLista').dialog({
            width: 600,
            modal: true,
            position: { my: "top", at: "top" },
            buttons: [{
                text: "Fechar",
                click: function () {
                    $(this).dialog("close");
                    $(this).dialog("destroy");
                    $('#FuncionarioLista').hide();
                },
            }]
        });
    });
}

function listarFuncionarios() {

    $('#FuncionarioLista').jtable({
        title: 'Funcionários',
        columnResizable: true,
        paging: true,
        pageSize: 10,
        sorting: false,
        actions: {
            listAction: '/Funcionario/Listar',
        },
        fields: {
            Editar: {
                title: 'Selecionar',
                sorting: false,
                width: '5%',
                display: function (data) {
                    return '<a href="javascript:selecionarGestor(' + data.record.IdFuncionario + ',\'' + data.record.Nome + '\');">Selecionar</a>';
                },
            },
            Nome: {
                title: 'Name',
                width: '30%'
            },
            Cargo: {
                title: 'Cargo',
                width: '20%'
            },
            Departamento: {
                title: 'Departamento',
                width: '30%'
            }
        }
    });

    $('#btnPesquisar').click(function (e) {
        e.preventDefault();
        $('#FuncionarioLista').jtable('load', {
            pesquisa: $('#pesquisa').val()
        });
    });
    $('#btnPesquisar').click();
}

function mensagemOk() {
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
            "Novo funcionário":
                function () {
                    location.href = '/Funcionario/Cadastrar/0';
                }
        }
    });
    });
}
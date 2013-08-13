
$(document).ready(function () {
    $('#FuncionarioLista').jtable({
        title: 'Funcionários',
        columnResizable: false,
        paging: true,
        pageSize: 10,
        sorting: false,
        actions: {
            listAction: '/Funcionario/Listar',
        },
        fields: {
            Editar: {
                title: '',
                sorting: false,
                width: '10%',
                display: function (data) {
                    return '<a href="?id=' + data.record.IdFuncionario + '">Editar</a>';
                },
            },
            Visualizar: {
                title: '',
                sorting: false,
                width: '5%',
                display: function (data) {
                    return '<a href="/Funcionario/Visualizar/' + data.record.IdFuncionario + '">Visualizar</a>';
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
            },
            Ativo: {
                title: 'Status',
                width: '5%',
                sorting: false
            },
        }
    });

    $('#btnPesquisar').click(function (e) {
        e.preventDefault();
        $('#FuncionarioLista').jtable('load', {
            pesquisa: $('#pesquisa').val()
        });
    });

    $('#btnPesquisar').click();
});
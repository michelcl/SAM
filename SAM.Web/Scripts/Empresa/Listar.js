
$(document).ready(function () {

    $.extend(true, $.hik.jtable.prototype.options.messages, {
        addNewRecord: 'Adicionar nova unidade',
    });

    $.extend(true, $.hik.jtable.prototype, {
        _createAddRecordDialogDiv: function () {
            var self = this;
            //Check if createAction is supplied
            if (!self.options.actions.createAction) {
                return;
            }

            if (self.options.addRecordButton) {
                //If user supplied a button, bind the click event to show dialog form
                self.options.addRecordButton.click(function (e) {
                    e.preventDefault();
                    window.location.href = self.options.actions.createAction;
                });
            } else {
                //If user did not supplied a button, create a 'add record button' toolbar item.
                self._addToolBarItem({
                    icon: true,
                    cssClass: 'jtable-toolbar-item-add-record',
                    text: self.options.messages.addNewRecord,
                    click: function () {
                        window.location.href = self.options.actions.createAction;
                    }
                });
            }
        }
    });




    $('#EmpresaLista').jtable({
        title: 'Organizações',
        columnResizable: false,
        paging: true,
        pageSize: 10,
        sorting: false,
        actions: {
            listAction: '/Empresa/Listar',
        },
        fields: {
            Editar: {
                title: '',
                sorting: false,
                width: '5%',
                display: function (data) {
                    return '<a href="/Empresa/Cadastrar/' + data.record.IdEmpresa + '">Editar</a>';
                },
            },
            Visualizar: {
                title: '',
                sorting: false,
                width: '5%',
                display: function (data) {
                    return '<a href="/Empresa/Visualizar/' + data.record.IdEmpresa + '">Visualizar</a>';
                },
            },
            Filial: {
                title: '',
                width: '5%',
                sorting: false,
                edit: false,
                create: false,
                display: function (empresa) {
                    //Create an image that will be used to open child table
                    var $img = $('<a href="javascript:;">Unidades</a>');
                    //Open child table when user clicks the image
                    $img.click(function () {
                        $('#EmpresaLista').jtable('openChildTable',
                                $img.closest('tr'),
                                {
                                    title: 'Unidades - ' + empresa.record.NomeFantasia + ' ' + empresa.record.Cnpj,
                                    columnResizable: false,
                                    actions: {
                                        listAction: '/Filial/Listar?IdEmpresa=' + empresa.record.IdEmpresa,
                                        createAction: '/Filial/Cadastrar?IdEmpresa=' + empresa.record.IdEmpresa,
                                    },
                                    fields: {
                                        Editar: {
                                            title: '',
                                            sorting: false,
                                            width: '5%',
                                            display: function (data) {
                                                return '<a href="/Filial/Cadastrar/' + data.record.IdFilial + '">Editar</a>';
                                            },
                                        },
                                        Visualizar: {
                                            title: '',
                                            sorting: false,
                                            width: '5%',
                                            display: function (data) {
                                                return '<a href="/Filial/Visualizar/' + data.record.IdFilial + '">Visualizar</a>';
                                            },
                                        },
                                        Cnpj: {
                                            title: 'CNPJ',
                                            width: '20%'
                                        },
                                        NomeFantasia: {
                                            title: 'Nome Fantasia',
                                            width: '35%'
                                        },
                                        RazaoSocial: {
                                            title: 'Razão Social',
                                            width: '40%'
                                        },
                                        Ativo: {
                                            title: 'Status',
                                            width: '5%',
                                            sorting: false
                                        },
                                    }
                                }, function (data) { //opened handler
                                    data.childTable.jtable('load');
                                });
                    });
                    //Return image to show on the person row
                    return $img;
                }
            },
            Cnpj: {
                title: 'CNPJ',
                width: '20%'
            },
            NomeFantasia: {
                title: 'Nome Fantasia',
                width: '20%'
            },
            RazaoSocial: {
                title: 'Razão Social',
                width: '20%'
            },
            Segmento: {
                title: 'Segmento',
                width: '20%'
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
        $('#EmpresaLista').jtable('load', {
            pesquisa: $('#pesquisa').val()
        });
    });

    $('#btnPesquisar').click();
});
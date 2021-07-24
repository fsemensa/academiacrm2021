function Onload(executionContext) {
    if (Xrm.Page.ui.getFormType() == 2) {
        debugger;
        //var contato = Xrm.Page.getAttribute("primarycontactid").getValue();
        var formContext = executionContext.getFormContext();
        var contato = formContext.data.entity.attributes.getByName('primarycontactid');
        if (contato !== null) {
            var contactId = contato.getValue()[0].id;
            Retrieve(contactId);

            debugger;
            var idConta = formContext.data.entity._entityId;
            CriarContato2(idConta, contato.getValue()[0].name, formContext.data.entity.attributes.getByName('name').getValue());
        }
    }
}

function Retrieve(contactId) {
    debugger;
    Xrm.WebApi.retrieveRecord("contact", contactId, "?$select=fullname").then(
        function success(resulContato) {
            Xrm.Navigation.openAlertDialog(resulContato.fullname);
        },
        function (error) {
            Xrm.Navigation.openAlertDialog(error.message);
        }
    );
}

function CriarContato2(accountid, firstname, lastname) {
    debugger;
    var entity = {};
    entity["parentcustomerid_account@odata.bind"] = "/accounts(" + accountid.guid + ")";
    entity.firstname = firstname;
    entity.lastname = lastname;
    Xrm.WebApi.online.createRecord("contact", entity).then(
        function success(result) {
            UpdateContato(result.id, firstname, lastname);
            Xrm.Navigation.openAlertDialog("Contato Criado: " + result.id);
        },
        function (error) {
            Xrm.Utility.alertDialog(error.message);
        }
    );
}

function UpdateContato(contactId, firstname, lastname) {
    debugger;
    var data = { "firstname": "Update " + firstname, "lastname": lastname };
    Xrm.WebApi.updateRecord("contact", contactId, data).then(
        function success(result) {
            Xrm.Navigation.openAlertDialog("Contato atualizado");
        },
        function (error) {
            Xrm.Navigation.openAlertDialog(error.message);
        }
    );
}

function DeleteContato(contactId) {
    debugger;
    Xrm.WebApi.deleteRecord("contact", contactId).then(
        function success(result) {
            Xrm.Navigation.openAlertDialog("Contato apagado");
        },
        function (error) {
            Xrm.Navigation.openAlertDialog(error.messge);
        }
    );
}
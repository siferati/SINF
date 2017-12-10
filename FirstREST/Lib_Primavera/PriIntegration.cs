using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interop.ErpBS900;
using Interop.StdPlatBS900;
using Interop.StdBE900;
using Interop.GcpBE900;
using ADODB;
using Interop.CrmBE900;
using System.Diagnostics;

namespace FirstREST.Lib_Primavera
{
    public class PriIntegration
    {
        

        # region Cliente

        public static List<Model.Cliente> ListaClientes()
        {

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                // list of clients
                List<Model.Cliente> listClientes = new List<Model.Cliente>();

                // get id of all clientes
                StdBELista queryResult = PriEngine.Engine.Consulta(@"
                    SELECT Cliente
                    FROM Clientes
                ");

                while (!queryResult.NoFim())
                {
                    // add new cliente
                    listClientes.Add(GetCliente(queryResult.Valor("Cliente")));

                    // next ite
                    queryResult.Seguinte();
                }

                return listClientes;

            }
            else
            {
                return null;

            }
        }

        public static Lib_Primavera.Model.Cliente GetCliente(string codCliente)
        {
            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                // get info about given product
                StdBELista queryResult = PriEngine.Engine.Consulta(@"
                    SELECT Cliente, NumContrib, Nome, Fac_Mor, Fac_Mor2, Fac_Tel, CDU_Email, Notas
                    FROM Clientes
                    WHERE Cliente = '" + codCliente + "'"
                );

                if (!queryResult.Vazia())
                {
                    // return product
                    return new Model.Cliente
                    {
                        customerId = queryResult.Valor("Cliente"),
                        fiscalId = queryResult.Valor("NumContrib"),
                        name = queryResult.Valor("Nome"),
                        address = queryResult.Valor("Fac_Mor") + " " + queryResult.Valor("Fac_Mor2"),
                        phone = queryResult.Valor("Fac_Tel"),
                        email = queryResult.Valor("CDU_Email"),
                        // TODO
                        status = "N/A",
                        orders = GetNOrdersByClient(codCliente),
                        description = queryResult.Valor("Notas"),
                        // TODO
                        picture = ""

                    };
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;

            }
        }

        public static int GetLastClientId()
        {
            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                // get max id
                StdBELista queryResult = PriEngine.Engine.Consulta(@"
                    SELECT MAX(Cliente) AS Id
                    FROM Clientes
                    WHERE ISNUMERIC(Cliente) = 1
                ");

                if (!queryResult.Vazia())
                {
                    // return product
                    string id = queryResult.Valor("Id");

                    if (String.IsNullOrEmpty(id))
                        return 0;
                    else
                        return int.Parse(id);
                }
                else
                {
                    return 0;
                }

            }
            else
            {
                return 0;

            }
        }

        public static Lib_Primavera.Model.RespostaErro InsereClienteObj(Model.Cliente cli)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();

            // initialize client
            GcpBECliente myCli = new GcpBECliente();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    // campos de utilizador
                    StdBECampos cdu = new StdBECampos();

                    // cdu_email
                    StdBECampo email = new StdBECampo();
                    email.Nome = "CDU_Email";
                    email.Valor = cli.email;
                    cdu.Insere(email);
                    
                    // set client fields
                    myCli.set_Cliente("" + (GetLastClientId() + 1));
                    myCli.set_NumContribuinte(cli.fiscalId);
                    myCli.set_Nome(cli.name);
                    if (cli.address.Length <= 50)
                    {
                        myCli.set_Morada(cli.address);
                    }
                    else
                    {
                        myCli.set_Morada(cli.address.Substring(0, 50));
                        myCli.set_Morada2(cli.address.Substring(50, cli.address.Length - 50));
                    }                    
                    myCli.set_Telefone(cli.phone);
                    myCli.set_CamposUtil(cdu);
                    myCli.set_Observacoes(cli.description);
                    myCli.set_Moeda("EUR");

                    // insert client
                    PriEngine.Engine.Comercial.Clientes.Actualiza(myCli);

                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }


        }

        public static Lib_Primavera.Model.RespostaErro UpdCliente(String id, Lib_Primavera.Model.Cliente cliente)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();

            // initialize client
            GcpBECliente objCli = new GcpBECliente();

            try
            {

                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    if (PriEngine.Engine.Comercial.Clientes.Existe(id) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        objCli = PriEngine.Engine.Comercial.Clientes.Edita(id);
                        objCli.set_EmModoEdicao(true);

                        // campos de utilizador
                        StdBECampos cdu = new StdBECampos();

                        // cdu_email
                        StdBECampo email = new StdBECampo();
                        email.Nome = "CDU_Email";
                        email.Valor = cliente.email;
                        cdu.Insere(email);

                        // set client fields
                        objCli.set_Nome(cliente.name);
                        if (cliente.address.Length <= 50)
                        {
                            objCli.set_Morada(cliente.address);
                        }
                        else
                        {
                            objCli.set_Morada(cliente.address.Substring(0, 50));
                            objCli.set_Morada2(cliente.address.Substring(50, cliente.address.Length - 50));
                        }
                        objCli.set_Telefone(cliente.phone);
                        objCli.set_CamposUtil(cdu);
                        objCli.set_Observacoes(cliente.description);

                        // update client
                        PriEngine.Engine.Comercial.Clientes.Actualiza(objCli);

                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;

                }

            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }

        /*
        public static Lib_Primavera.Model.RespostaErro DelCliente(string codCliente)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBECliente objCli = new GcpBECliente();


            try
            {

                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        PriEngine.Engine.Comercial.Clientes.Remove(codCliente);
                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }

                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }

       */

        #endregion Cliente;   // -----------------------------  END   CLIENTE    -----------------------


        #region Artigo

        public static Lib_Primavera.Model.Artigo GetArtigo(string codArtigo)
        {
            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                // get info about given product
                StdBELista queryResult = PriEngine.Engine.Consulta(@"
                    SELECT Artigo, Descricao, Iva, STKActual, Peso, CDU_Preco, CDU_Tipo, CDU_Tamanho, CDU_Descricao
                    FROM Artigo
                    WHERE Artigo = '" + codArtigo + "'"
                );

                if (!queryResult.Vazia())
                {
                    // return product
                    return new Model.Artigo
                    {
                        productId = queryResult.Valor("Artigo"),
                        name = queryResult.Valor("Descricao"),
                        price = queryResult.Valor("CDU_Preco"),
                        VAT = queryResult.Valor("Iva"),
                        size = queryResult.Valor("CDU_Tamanho"),
                        type = queryResult.Valor("CDU_Tipo"),
                        stock = queryResult.Valor("STKActual"),
                        weight = queryResult.Valor("Peso"),
                        description = queryResult.Valor("CDU_Descricao")
                    };
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;

            }

        }

        public static List<Model.Artigo> ListaArtigos()
        {

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                // list of artigos
                List<Model.Artigo> listArtigos = new List<Model.Artigo>();

                // get id of all artigos
                StdBELista queryResult = PriEngine.Engine.Consulta(@"
                    SELECT Artigo
                    FROM Artigo
                ");

                while (!queryResult.NoFim())
                {
                    // add new artigo
                    listArtigos.Add(GetArtigo(queryResult.Valor("Artigo")));

                    // next ite
                    queryResult.Seguinte();
                }

                return listArtigos;

            }
            else
            {
                return null;

            }
        }

        #endregion Artigo


        #region Vendedor

        public static Lib_Primavera.Model.Vendedor GetVendedor(string codVendedor)
        {
            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                // get info about given sales rep
                StdBELista queryResult = PriEngine.Engine.Consulta(@"
                    SELECT Vendedor, CDU_NumContr, Nome, Morada, Telemovel, EMail, CDU_DataNascimento, CDU_DataAdmissao, Notas
                    FROM Vendedores
                    WHERE Vendedor = '" + codVendedor + "'"
                );

                if (!queryResult.Vazia())
                {
                    // return sales rep
                    return new Model.Vendedor
                    {
                        repId = queryResult.Valor("Vendedor"),
                        fiscalId = queryResult.Valor("CDU_NumContr"),
                        name = queryResult.Valor("Nome"),
                        address = queryResult.Valor("Morada"),
                        phone = queryResult.Valor("Telemovel"),
                        email = queryResult.Valor("EMail"),
                        birthDate = queryResult.Valor("CDU_DataNascimento"),
                        hiredDate = queryResult.Valor("CDU_DataAdmissao"),
                        sales = GetNOrdersByRep(codVendedor),
                        description = queryResult.Valor("Notas"),
                        // TODO foto
                        picture = ""
                    };
                }

                else
                {
                    return null;
                }

            }
            else
            {
                return null;

            }
            

        }

        public static List<Model.Vendedor> ListaVendedores()
        {
            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                // list of sales reps
                List<Model.Vendedor> listVendedores = new List<Model.Vendedor>();

                // get id of all sales reps
                StdBELista queryResult = PriEngine.Engine.Consulta(@"
                    SELECT Vendedor
                    FROM Vendedores
                ");

                while (!queryResult.NoFim())
                {
                    // add new sales rep
                    listVendedores.Add(GetVendedor(queryResult.Valor("Vendedor")));

                    // next ite
                    queryResult.Seguinte();
                }

                return listVendedores;

            }
            else
            {
                return null;

            }
        }

        public static int GetLastVendedorId()
        {
            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                // get max id
                StdBELista queryResult = PriEngine.Engine.Consulta(@"
                    SELECT MAX(Vendedor) AS Id
                    FROM Vendedores
                    WHERE ISNUMERIC(Vendedor) = 1
                ");

                if (!queryResult.Vazia())
                {
                    // return product
                    string id = queryResult.Valor("Id");

                    if (String.IsNullOrEmpty(id))
                        return 0;
                    else
                        return int.Parse(id);
                }
                else
                {
                    return 0;
                }

            }
            else
            {
                return 0;

            }
        }

        public static Lib_Primavera.Model.RespostaErro InsereVendedorObj(Model.Vendedor vend)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();

            // initialize rep
            GcpBEVendedor myVend = new GcpBEVendedor();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    // campos de utilizador
                    StdBECampos cdu = new StdBECampos();

                    // cdu_dataNascimento
                    StdBECampo dataNascimento = new StdBECampo();
                    dataNascimento.Nome = "CDU_DataNascimento";
                    dataNascimento.Valor = vend.birthDate;
                    cdu.Insere(dataNascimento);

                    // cdu_dataAdmissao
                    StdBECampo dataAdmissao = new StdBECampo();
                    dataAdmissao.Nome = "CDU_DataAdmissao";
                    dataAdmissao.Valor = vend.hiredDate;
                    cdu.Insere(dataAdmissao);

                    // cdu_numcontrb
                    StdBECampo numcontrb = new StdBECampo();
                    numcontrb.Nome = "CDU_NumContr";
                    numcontrb.Valor = vend.fiscalId;
                    cdu.Insere(numcontrb);

                    // set vend fields
                    myVend.set_CamposUtil(cdu);
                    myVend.set_Vendedor("" + (GetLastVendedorId() + 1));
                    myVend.set_Nome(vend.name);
                    myVend.set_Morada(vend.address);
                    myVend.set_Telefone(vend.phone);
                    myVend.set_Email(vend.email);
                    myVend.set_Observacoes(vend.description);
                    myVend.set_Moeda("EUR");

                    // insert vend
                    PriEngine.Engine.Comercial.Vendedores.Actualiza(myVend);

                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }


        }

        public static Lib_Primavera.Model.RespostaErro UpdVendedor(String id, Lib_Primavera.Model.Vendedor vendedor)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();

            // initialize vendedor
            GcpBEVendedor objVend = new GcpBEVendedor();

            try
            {

                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    if (PriEngine.Engine.Comercial.Vendedores.Existe(id) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O vendedor não existe";
                        return erro;
                    }
                    else
                    {

                        objVend = PriEngine.Engine.Comercial.Vendedores.Edita(id);
                        objVend.set_EmModoEdicao(true);

                        // campos de utilizador
                        StdBECampos cdu = new StdBECampos();

                        // cdu_dataNascimento
                        StdBECampo dataNascimento = new StdBECampo();
                        dataNascimento.Nome = "CDU_DataNascimento";
                        dataNascimento.Valor = vendedor.birthDate;
                        cdu.Insere(dataNascimento);

                        // cdu_dataAdmissao
                        StdBECampo dataAdmissao = new StdBECampo();
                        dataAdmissao.Nome = "CDU_DataAdmissao";
                        dataAdmissao.Valor = vendedor.hiredDate;
                        cdu.Insere(dataAdmissao);

                        // set vend fields
                        objVend.set_CamposUtil(cdu);
                        objVend.set_Nome(vendedor.name);
                        objVend.set_Morada(vendedor.address);
                        objVend.set_Telefone(vendedor.phone);
                        objVend.set_Email(vendedor.email);
                        objVend.set_Observacoes(vendedor.description);

                        // update client
                        PriEngine.Engine.Comercial.Vendedores.Actualiza(objVend);

                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;

                }

            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }

        #endregion Vendedor


        #region Order

        public static Model.Order GetOrder(string id)
        {
            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                // get info about given sales order (CabecDoc)
                StdBELista cabecQueryResult = PriEngine.Engine.Consulta(@"
                    SELECT Id, Entidade, Responsavel, MoradaEntrega, Morada2Entrega, Data, DataDescarga
                    FROM CabecDoc
                    WHERE Id = '" + id + @"'
                ");

                if (!cabecQueryResult.Vazia())
                {
                    // get info about given sales order (LinhasDoc)
                    StdBELista linhasQueryResult = PriEngine.Engine.Consulta(@"
                        SELECT Artigo, Quantidade
                        FROM LinhasDoc
                        WHERE IdCabecDoc = '" + id + @"'
                    ");

                    // sales order to return
                    Model.Order order = new Model.Order
                    {
                        salesOrderId = cabecQueryResult.Valor("Id"),
                        customerId = cabecQueryResult.Valor("Entidade"),
                        repId = cabecQueryResult.Valor("Responsavel"),
                        products = new List<Model.Product>(),
                        orderDate = cabecQueryResult.Valor("Data"),
                        // TOOD
                        status = "N/A",
                        deliveryAddress = cabecQueryResult.Valor("MoradaEntrega") + " " + cabecQueryResult.Valor("Morada2Entrega")
                    };

                    // delivery date might be null, so this needs to be outside order initialization
                    if (!String.IsNullOrEmpty(cabecQueryResult.Valor("DataDescarga")))
                    {
                        order.deliveryDate = DateTime.Parse(cabecQueryResult.Valor("DataDescarga"));
                    }

                    // iterate through every line
                    while (!linhasQueryResult.NoFim())
                    {
                        // add ordered product to sales order
                        order.products.Add(new Model.Product
                        {
                            productId = linhasQueryResult.Valor("Artigo"),
                            quantity = linhasQueryResult.Valor("Quantidade")
                        });

                        // next ite
                        linhasQueryResult.Seguinte();
                    }                    

                    return order;
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;

            }
        }

        public static List<Model.Order> GetOrdersByRep(string id)
        {
            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                // list of sales orders
                List<Model.Order> listOrders = new List<Model.Order>();

                // get id of all sales orders for given rep
                StdBELista queryResult = PriEngine.Engine.Consulta(@"
                    SELECT Id
                    FROM CabecDoc
                    WHERE Responsavel = '" + id + @"'
                ");

                while (!queryResult.NoFim())
                {
                    // add sales rep
                    listOrders.Add(GetOrder(queryResult.Valor("Id")));

                    // next ite
                    queryResult.Seguinte();

                }

                return listOrders;

            }
            else
            {
                return null;

            }
        }

        public static int GetNOrdersByRep(string id)
        {
            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                // get id of all sales orders for given rep
                StdBELista queryResult = PriEngine.Engine.Consulta(@"
                    SELECT COUNT(*) AS Norders
                    FROM CabecDoc
                    WHERE Responsavel = '" + id + @"'
                ");

                if (!queryResult.Vazia())
                {
                    return queryResult.Valor("Norders");

                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;

            }
        }

        public static List<Model.Order> GetOrdersByClient(string id)
        {
            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                // list of sales orders
                List<Model.Order> listOrders = new List<Model.Order>();

                // get id of all sales orders for given client
                StdBELista queryResult = PriEngine.Engine.Consulta(@"
                    SELECT Id
                    FROM CabecDoc
                    WHERE Entidade = '" + id + @"'
                ");

                while (!queryResult.NoFim())
                {
                    // add sales rep
                    listOrders.Add(GetOrder(queryResult.Valor("Id")));

                    // next ite
                    queryResult.Seguinte();

                }

                return listOrders;

            }
            else
            {
                return null;

            }
        }

        public static int GetNOrdersByClient(string id)
        {
            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                // get id of all sales orders for given client
                StdBELista queryResult = PriEngine.Engine.Consulta(@"
                    SELECT COUNT(*) AS Norders
                    FROM CabecDoc
                    WHERE Entidade = '" + id + @"'
                ");

                if (!queryResult.Vazia())
                {
                    return queryResult.Valor("Norders");

                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;

            }
        }


        #endregion Order


        #region DbInfo

        public static List<Model.DbInfo> getDbInfo()
        {
            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                // returns queried information
                List<Model.DbInfo> retorno = new List<Model.DbInfo>();

                // list of queries to perform
                List<StdBELista> listQueries = new List<StdBELista>();

                // get info sales
                //listQueries.Add(PriEngine.Engine.Comercial.Vendas.LstLinhasDocVendas("", new DateTime(2016, 1, 1), new DateTime(2017, 1, 1), "", "", ""));

                // get info for ALL sales
                /*listQueries.Add(PriEngine.Engine.Consulta(@"
                    SELECT * FROM DocumentosVenda
                "));*/

                // get cabecalho de factura
                /*listQueries.Add(PriEngine.Engine.Consulta(@"
                    SELECT Id, Entidade, Responsavel, MoradaEntrega, Morada2Entrega
                    FROM CabecDoc
                    WHERE Data = '2017-11-10'
                "));*/

                // get corpo de factura
                /*listQueries.Add(PriEngine.Engine.Consulta(@"
                    SELECT Artigo, Quantidade, Data, DataEntrega
                    FROM LinhasDoc
                    WHERE Data = '2017-11-10'
                "));*/

                // get factura of day 2016-11-16
                /*listQueries.Add(PriEngine.Engine.Consulta(@"
                    SELECT CabecDoc.Id, Entidade, Responsavel, MoradaEntrega, Morada2Entrega, Artigo, Quantidade, CabecDoc.Data, DataDescarga
                    FROM CabecDoc, LinhasDoc
                    WHERE CabecDoc.Id = LinhasDoc.IdCabecDoc
                    AND CabecDoc.Data = '2016-11-16'
                "));*/

                listQueries.Add(PriEngine.Engine.Consulta(@"
                    SELECT MAX(Cliente) AS Id
                    FROM Clientes
                    WHERE ISNUMERIC(Cliente) = 1
                "));

                foreach (StdBELista dbQuery in listQueries)
                {
                    while (!dbQuery.NoFim())
                    {
                        // holder for query result
                        Model.DbInfo dbInfo = new Model.DbInfo();
                        // initialize string[]
                        dbInfo.Result = new string[dbQuery.NumColunas()];
                        // get query
                        dbInfo.Query = dbQuery.Query;

                        for (short i = 0; i < dbQuery.NumColunas(); i++)
                        {
                            // col name
                            string colName = dbQuery.Nome(i);
                            // col value
                            string colVal = "" + dbQuery.Valor(colName);

                            // add to query result
                            dbInfo.Result[i] = colName + ": " + colVal;
                            if (String.IsNullOrEmpty(colVal))
                                dbInfo.Result[i] += "N/A";
                        }

                        // order result alphabetically
                        Array.Sort(dbInfo.Result);

                        // add to return list
                        retorno.Add(dbInfo);

                        // next ite
                        dbQuery.Seguinte();
                    }
                }

                return retorno;

            }
            else
            {
                return null;

            }

        }

        #endregion DbInfo


        #region DocCompra


        public static List<Model.DocCompra> VGR_List()
        {
                
            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocCompra dc = new Model.DocCompra();
            List<Model.DocCompra> listdc = new List<Model.DocCompra>();
            Model.LinhaDocCompra lindc = new Model.LinhaDocCompra();
            List<Model.LinhaDocCompra> listlindc = new List<Model.LinhaDocCompra>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT id, NumDocExterno, Entidade, DataDoc, NumDoc, TotalMerc, Serie From CabecCompras where TipoDoc='VGR'");
                while (!objListCab.NoFim())
                {
                    dc = new Model.DocCompra();
                    dc.id = objListCab.Valor("id");
                    dc.NumDocExterno = objListCab.Valor("NumDocExterno");
                    dc.Entidade = objListCab.Valor("Entidade");
                    dc.NumDoc = objListCab.Valor("NumDoc");
                    dc.Data = objListCab.Valor("DataDoc");
                    dc.TotalMerc = objListCab.Valor("TotalMerc");
                    dc.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecCompras, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido, Armazem, Lote from LinhasCompras where IdCabecCompras='" + dc.id + "' order By NumLinha");
                    listlindc = new List<Model.LinhaDocCompra>();

                    while (!objListLin.NoFim())
                    {
                        lindc = new Model.LinhaDocCompra();
                        lindc.IdCabecDoc = objListLin.Valor("idCabecCompras");
                        lindc.CodArtigo = objListLin.Valor("Artigo");
                        lindc.DescArtigo = objListLin.Valor("Descricao");
                        lindc.Quantidade = objListLin.Valor("Quantidade");
                        lindc.Unidade = objListLin.Valor("Unidade");
                        lindc.Desconto = objListLin.Valor("Desconto1");
                        lindc.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindc.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindc.TotalLiquido = objListLin.Valor("PrecoLiquido");
                        lindc.Armazem = objListLin.Valor("Armazem");
                        lindc.Lote = objListLin.Valor("Lote");

                        listlindc.Add(lindc);
                        objListLin.Seguinte();
                    }

                    dc.LinhasDoc = listlindc;
                    
                    listdc.Add(dc);
                    objListCab.Seguinte();
                }
            }
            return listdc;
        }

                
        public static Model.RespostaErro VGR_New(Model.DocCompra dc)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            

            GcpBEDocumentoCompra myGR = new GcpBEDocumentoCompra();
            GcpBELinhaDocumentoCompra myLin = new GcpBELinhaDocumentoCompra();
            GcpBELinhasDocumentoCompra myLinhas = new GcpBELinhasDocumentoCompra();

            Interop.CrmBE900.PreencheRelacaoCompras rl = new Interop.CrmBE900.PreencheRelacaoCompras();
            List<Model.LinhaDocCompra> lstlindv = new List<Model.LinhaDocCompra>();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    // Atribui valores ao cabecalho do doc
                    //myEnc.set_DataDoc(dv.Data);
                    myGR.set_Entidade(dc.Entidade);
                    myGR.set_NumDocExterno(dc.NumDocExterno);
                    myGR.set_Serie(dc.Serie);
                    myGR.set_Tipodoc("VGR");
                    myGR.set_TipoEntidade("F");
                    // Linhas do documento para a lista de linhas
                    lstlindv = dc.LinhasDoc;
                    //PriEngine.Engine.Comercial.Compras.PreencheDadosRelacionados(myGR,rl);
                    PriEngine.Engine.Comercial.Compras.PreencheDadosRelacionados(myGR);
                    foreach (Model.LinhaDocCompra lin in lstlindv)
                    {
                        PriEngine.Engine.Comercial.Compras.AdicionaLinha(myGR, lin.CodArtigo, lin.Quantidade, lin.Armazem, "", lin.PrecoUnitario, lin.Desconto);
                    }


                    PriEngine.Engine.IniciaTransaccao();
                    PriEngine.Engine.Comercial.Compras.Actualiza(myGR, "Teste");
                    PriEngine.Engine.TerminaTransaccao();
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;

                }

            }
            catch (Exception ex)
            {
                PriEngine.Engine.DesfazTransaccao();
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }


        #endregion DocCompra


        #region DocsVenda

        public static Model.RespostaErro Encomendas_New(Model.DocVenda dv)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBEDocumentoVenda myEnc = new GcpBEDocumentoVenda();
             
            GcpBELinhaDocumentoVenda myLin = new GcpBELinhaDocumentoVenda();

            GcpBELinhasDocumentoVenda myLinhas = new GcpBELinhasDocumentoVenda();
             
            Interop.CrmBE900.PreencheRelacaoVendas rl = new Interop.CrmBE900.PreencheRelacaoVendas();
            List<Model.LinhaDocVenda> lstlindv = new List<Model.LinhaDocVenda>();
            
            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    // Atribui valores ao cabecalho do doc
                    //myEnc.set_DataDoc(dv.Data);
                    myEnc.set_Entidade(dv.Entidade);
                    myEnc.set_Serie(dv.Serie);
                    myEnc.set_Tipodoc("ECL");
                    myEnc.set_TipoEntidade("C");
                    // Linhas do documento para a lista de linhas
                    lstlindv = dv.LinhasDoc;
                    //PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(myEnc, rl);
                    PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(myEnc);
                    foreach (Model.LinhaDocVenda lin in lstlindv)
                    {
                        PriEngine.Engine.Comercial.Vendas.AdicionaLinha(myEnc, lin.CodArtigo, lin.Quantidade, "", "", lin.PrecoUnitario, lin.Desconto);
                    }


                   // PriEngine.Engine.Comercial.Compras.TransformaDocumento(

                    PriEngine.Engine.IniciaTransaccao();
                    //PriEngine.Engine.Comercial.Vendas.Edita Actualiza(myEnc, "Teste");
                    PriEngine.Engine.Comercial.Vendas.Actualiza(myEnc, "Teste");
                    PriEngine.Engine.TerminaTransaccao();
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;

                }

            }
            catch (Exception ex)
            {
                PriEngine.Engine.DesfazTransaccao();
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }

     

        public static List<Model.DocVenda> Encomendas_List()
        {
            
            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            List<Model.DocVenda> listdv = new List<Model.DocVenda>();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new
            List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT id, Entidade, Data, NumDoc, TotalMerc, Serie From CabecDoc where TipoDoc='ECL'");
                while (!objListCab.NoFim())
                {
                    dv = new Model.DocVenda();
                    dv.id = objListCab.Valor("id");
                    dv.Entidade = objListCab.Valor("Entidade");
                    dv.NumDoc = objListCab.Valor("NumDoc");
                    dv.Data = objListCab.Valor("Data");
                    dv.TotalMerc = objListCab.Valor("TotalMerc");
                    dv.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dv.id + "' order By NumLinha");
                    listlindv = new List<Model.LinhaDocVenda>();

                    while (!objListLin.NoFim())
                    {
                        lindv = new Model.LinhaDocVenda();
                        lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                        lindv.CodArtigo = objListLin.Valor("Artigo");
                        lindv.DescArtigo = objListLin.Valor("Descricao");
                        lindv.Quantidade = objListLin.Valor("Quantidade");
                        lindv.Unidade = objListLin.Valor("Unidade");
                        lindv.Desconto = objListLin.Valor("Desconto1");
                        lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");

                        listlindv.Add(lindv);
                        objListLin.Seguinte();
                    }

                    dv.LinhasDoc = listlindv;
                    listdv.Add(dv);
                    objListCab.Seguinte();
                }
            }
            return listdv;
        }


       

        public static Model.DocVenda Encomenda_Get(string numdoc)
        {
            
            
            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                

                string st = "SELECT id, Entidade, Data, NumDoc, TotalMerc, Serie From CabecDoc where TipoDoc='ECL' and NumDoc='" + numdoc + "'";
                objListCab = PriEngine.Engine.Consulta(st);
                dv = new Model.DocVenda();
                dv.id = objListCab.Valor("id");
                dv.Entidade = objListCab.Valor("Entidade");
                dv.NumDoc = objListCab.Valor("NumDoc");
                dv.Data = objListCab.Valor("Data");
                dv.TotalMerc = objListCab.Valor("TotalMerc");
                dv.Serie = objListCab.Valor("Serie");
                objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dv.id + "' order By NumLinha");
                listlindv = new List<Model.LinhaDocVenda>();

                while (!objListLin.NoFim())
                {
                    lindv = new Model.LinhaDocVenda();
                    lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                    lindv.CodArtigo = objListLin.Valor("Artigo");
                    lindv.DescArtigo = objListLin.Valor("Descricao");
                    lindv.Quantidade = objListLin.Valor("Quantidade");
                    lindv.Unidade = objListLin.Valor("Unidade");
                    lindv.Desconto = objListLin.Valor("Desconto1");
                    lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                    lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                    lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");
                    listlindv.Add(lindv);
                    objListLin.Seguinte();
                }

                dv.LinhasDoc = listlindv;
                return dv;
            }
            return null;
        }

        #endregion DocsVenda

        #region OportunidadeVenda

        public static Lib_Primavera.Model.OportunidadeVenda GetOpVenda(string codOpVenda)
        {

            CrmBEOportunidadeVenda objOpVenda = new CrmBEOportunidadeVenda();
            Model.OportunidadeVenda myOpVenda = new Model.OportunidadeVenda();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                if (PriEngine.Engine.CRM.OportunidadesVenda.Existe(codOpVenda) == false)
                {
                    return null;
                }
                else
                {

                    StdBELista queryResult = PriEngine.Engine.Consulta(@"SELECT CabecOportunidadesVenda.Oportunidade, CabecOportunidadesVenda.Descricao, CabecOportunidadesVenda.Entidade as entidade, CabecOportunidadesVenda.Vendedor, CabecOportunidadesVenda.CDU_DataEncontro, CabecOportunidadesVenda.CDU_LocalEncontro
                    FROM CabecOportunidadesVenda 
                    JOIN Clientes ON (CabecOportunidadesVenda.Entidade = Clientes.Cliente)
                    WHERE CabecOportunidadesVenda.Oportunidade = '" + codOpVenda + "'"
                    );

                    Model.OportunidadeVenda objOPVenda = new Model.OportunidadeVenda();

                    objOPVenda.OportunidadeID = queryResult.Valor("Oportunidade");
                    objOPVenda.DescricaoOp = queryResult.Valor("Descricao");
                    objOPVenda.Entidade = queryResult.Valor("entidade");
                    objOPVenda.Data = queryResult.Valor("CDU_DataEncontro");
                    objOPVenda.Local = queryResult.Valor("CDU_LocalEncontro");
                    objOPVenda.VendedorCod = queryResult.Valor("Vendedor");

                    return objOPVenda;
                }

            }
            else
            {
                return null;
            }

        }

        public static List<Model.OportunidadeVenda> ListaOpVenda()
        {

            Model.OportunidadeVenda myOpVenda = new Model.OportunidadeVenda();
            List<Model.OportunidadeVenda> listOpsVenda = new List<Model.OportunidadeVenda>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                List<Model.OportunidadeVenda> listOpVenda = new List<Model.OportunidadeVenda>();



                StdBELista queryResult = PriEngine.Engine.Consulta(@"SELECT Oportunidade
                    FROM CabecOportunidadesVenda
                ");

                while (!queryResult.NoFim())
                {
                    listOpVenda.Add(GetOpVenda(queryResult.Valor("Oportunidade")));

                    queryResult.Seguinte();
                }

                return listOpVenda;

            }
            else
            {
                return null;

            }

        }

        public static Lib_Primavera.Model.RespostaErro InsereOpVenda(Model.OportunidadeVenda opVenda)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();

            CrmBEOportunidadeVenda objOpVenda = new CrmBEOportunidadeVenda();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    StdBECampos cdu = new StdBECampos();

                    // cdu_local
                    StdBECampo local = new StdBECampo();
                    local.Nome = "CDU_LocalEncontro";
                    local.Valor = opVenda.Local;
                    cdu.Insere(local);

                    // cdu_data
                    StdBECampo data = new StdBECampo();
                    data.Nome = "CDU_DataEncontro";
                    data.Valor = opVenda.Data;
                    cdu.Insere(data);

                    objOpVenda.set_Oportunidade(opVenda.OportunidadeID);
                    objOpVenda.set_Descricao(opVenda.DescricaoOp);
                    objOpVenda.set_Entidade(opVenda.Entidade);
                    objOpVenda.set_Vendedor(opVenda.VendedorCod);
                    objOpVenda.set_DataCriacao(DateTime.Now);
                    objOpVenda.set_TipoEntidade("C");
                    objOpVenda.set_DataExpiracao((DateTime)opVenda.Data);
                    objOpVenda.set_CicloVenda("CV_SOFT");
                    objOpVenda.set_Moeda("EUR");
                    objOpVenda.set_CamposUtil(cdu);

                    PriEngine.Engine.CRM.OportunidadesVenda.Actualiza(ref objOpVenda);
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                Debug.WriteLine(ex);
                return erro;
            }
        }


        #endregion OportunidadeVenda

    }
}
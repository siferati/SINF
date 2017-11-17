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

namespace FirstREST.Lib_Primavera
{
    public class PriIntegration
    {
        

        # region Cliente

        public static List<Model.Cliente> ListaClientes()
        {
            
            
            StdBELista objList;

            List<Model.Cliente> listClientes = new List<Model.Cliente>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                //objList = PriEngine.Engine.Comercial.Clientes.LstClientes();

                objList = PriEngine.Engine.Consulta("SELECT Cliente, Nome, Moeda, NumContrib as NumContribuinte, Fac_Mor AS campo_exemplo, CDU_Email as Email FROM  CLIENTES");

                
                while (!objList.NoFim())
                {
                    listClientes.Add(new Model.Cliente
                    {
                        CodCliente = objList.Valor("Cliente"),
                        NomeCliente = objList.Valor("Nome"),
                        Moeda = objList.Valor("Moeda"),
                        NumContribuinte = objList.Valor("NumContribuinte"),
                        Morada = objList.Valor("campo_exemplo"),
                        Email = objList.Valor("Email")
                    });
                    objList.Seguinte();

                }

                return listClientes;
            }
            else
                return null;
        }

        public static Lib_Primavera.Model.Cliente GetCliente(string codCliente)
        {
            

            GcpBECliente objCli = new GcpBECliente();


            Model.Cliente myCli = new Model.Cliente();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == true)
                {
                    
                    objCli = PriEngine.Engine.Comercial.Clientes.Edita(codCliente);
                    myCli.CodCliente = objCli.get_Cliente();
                    myCli.NomeCliente = objCli.get_Nome();
                    myCli.Moeda = objCli.get_Moeda();
                    myCli.NumContribuinte = objCli.get_NumContribuinte();
                    myCli.Morada = objCli.get_Morada();
                    myCli.Email = PriEngine.Engine.Comercial.Clientes.DaValorAtributo(codCliente, "CDU_Email");

                    
                    return myCli;
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }

        public static Lib_Primavera.Model.RespostaErro UpdCliente(Lib_Primavera.Model.Cliente cliente)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
           

            GcpBECliente objCli = new GcpBECliente();

            try
            {

                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    if (PriEngine.Engine.Comercial.Clientes.Existe(cliente.CodCliente) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        objCli = PriEngine.Engine.Comercial.Clientes.Edita(cliente.CodCliente);
                        objCli.set_EmModoEdicao(true);

                        objCli.set_Nome(cliente.NomeCliente);
                        objCli.set_NumContribuinte(cliente.NumContribuinte);
                        objCli.set_Moeda(cliente.Moeda);
                        objCli.set_Morada(cliente.Morada);
                        
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



        public static Lib_Primavera.Model.RespostaErro InsereClienteObj(Model.Cliente cli)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            

            GcpBECliente myCli = new GcpBECliente();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    myCli.set_Cliente(cli.CodCliente);
                    myCli.set_Nome(cli.NomeCliente);
                    myCli.set_NumContribuinte(cli.NumContribuinte);
                    myCli.set_Moeda(cli.Moeda);
                    myCli.set_Morada(cli.Morada);

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

       

        #endregion Cliente;   // -----------------------------  END   CLIENTE    -----------------------


        #region Artigo

        public static Lib_Primavera.Model.Artigo GetArtigo(string codArtigo)
        {
            
            GcpBEArtigo objArtigo = new GcpBEArtigo();
            Model.Artigo myArt = new Model.Artigo();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                if (PriEngine.Engine.Comercial.Artigos.Existe(codArtigo) == false)
                {
                    return null;
                }
                else
                {
                    objArtigo = PriEngine.Engine.Comercial.Artigos.Edita(codArtigo);
                    myArt.CodArtigo = objArtigo.get_Artigo();
                    myArt.DescArtigo = objArtigo.get_Descricao();
                    myArt.STKAtual = objArtigo.get_StkActual(); 

                    return myArt;
                }
                
            }
            else
            {
                return null;
            }

        }

        public static List<Model.Artigo> ListaArtigos()
        {
                        
            StdBELista objList;

            Model.Artigo art = new Model.Artigo();
            List<Model.Artigo> listArts = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();

                while (!objList.NoFim())
                {
                    art = new Model.Artigo();
                    art.CodArtigo = objList.Valor("artigo");
                    art.DescArtigo = objList.Valor("descricao");
                    //art.STKAtual = objList.Valor("stkatual");
                    
                    listArts.Add(art);
                    objList.Seguinte();
                }

                return listArts;

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
                    SELECT Codigo, NumContr, Nome, Morada, Telemovel, Email, DataNascimento, DataAdmissao, Notas, Foto
                    FROM Funcionarios, Cargos
                    WHERE Cargos.Descricao = 'Vendedor'
                    AND Cargos.Cargo = Funcionarios.CargoPrincipal
                    AND Codigo = '" + codVendedor + "'"
                );

                if (!queryResult.Vazia())
                {
                    // return sales rep
                    return new Model.Vendedor
                    {
                        repId = queryResult.Valor("Codigo"),
                        fiscalId = queryResult.Valor("NumContr"),
                        name = queryResult.Valor("Nome"),
                        address = queryResult.Valor("Morada"),
                        phone = queryResult.Valor("Telemovel"),
                        email = queryResult.Valor("Email"),
                        birthDate = queryResult.Valor("DataNascimento"),
                        hiredDate = queryResult.Valor("DataAdmissao"),
                        // TODO sales count
                        sales = 0,
                        description = queryResult.Valor("Notas"),
                        picture = queryResult.Valor("Foto")
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

                // get info about all sales reps
                StdBELista queryResult = PriEngine.Engine.Consulta(@"
                    SELECT Codigo, NumContr, Nome, Morada, Telemovel, Email, DataNascimento, DataAdmissao, Notas, Foto
                    FROM Funcionarios, Cargos
                    WHERE Cargos.Descricao = 'Vendedor'
                    AND Cargos.Cargo = Funcionarios.CargoPrincipal
                ");

                while (!queryResult.NoFim())
                {
                    // add new sales rep
                    listVendedores.Add(new Model.Vendedor
                    {
                        repId = queryResult.Valor("Codigo"),
                        fiscalId = queryResult.Valor("NumContr"),
                        name = queryResult.Valor("Nome"),
                        address = queryResult.Valor("Morada"),
                        phone = queryResult.Valor("Telemovel"),
                        email = queryResult.Valor("Email"),
                        birthDate = queryResult.Valor("DataNascimento"),
                        hiredDate = queryResult.Valor("DataAdmissao"),
                        // TODO sales count
                        sales = 0,
                        description = queryResult.Valor("Notas"),
                        picture = queryResult.Valor("Foto")
                    });

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
                        deliveryAddress = cabecQueryResult.Valor("MoradaEntrega") + cabecQueryResult.Valor("Morada2Entrega")
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
                listQueries.Add(PriEngine.Engine.Consulta(@"
                    SELECT CabecDoc.Id, Entidade, Responsavel, MoradaEntrega, Morada2Entrega, Artigo, Quantidade, CabecDoc.Data, DataDescarga
                    FROM CabecDoc, LinhasDoc
                    WHERE CabecDoc.Id = LinhasDoc.IdCabecDoc
                    AND CabecDoc.Data = '2016-11-16'
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

                    StdBELista queryResult = PriEngine.Engine.Consulta(@"SELECT CabecOportunidadesVenda.Oportunidade, CabecOportunidadesVenda.Descricao, CabecOportunidadesVenda.DataCriacao, CabecOportunidadesVenda.TipoEntidade, CabecOportunidadesVenda.Entidade as entidade, CabecOportunidadesVenda.Vendedor, Zonas.Descricao as zona , Clientes.Nome, Clientes.Fac_Mor, Clientes.Fac_Tel
                    FROM CabecOportunidadesVenda 
                    LEFT JOIN Zonas ON(CabecOportunidadesVenda.Zona = Zonas.Zona)
                    JOIN Clientes ON (CabecOportunidadesVenda.Entidade = Clientes.Cliente)
                    WHERE CabecOportunidadesVenda.Oportunidade = '" + codOpVenda + "'"
                    );

                    Model.OportunidadeVenda objOPVenda = new Model.OportunidadeVenda();

                    objOPVenda.OportunidadeID = queryResult.Valor("Oportunidade");
                    objOPVenda.DescricaoOp = queryResult.Valor("Descricao");
                    objOPVenda.TipoEntidade = queryResult.Valor("TipoEntidade");
                    objOPVenda.Entidade = queryResult.Valor("entidade");
                    objOPVenda.Data = queryResult.Valor("DataCriacao");
                    objOPVenda.Zona = queryResult.Valor("zona");
                    objOPVenda.Nome = queryResult.Valor("Nome");
                    objOPVenda.Morada = queryResult.Valor("Fac_Mor");
                    objOPVenda.Telemovel = queryResult.Valor("Fac_Mor");
                    objOPVenda.VendedorCod = queryResult.Valor("Vendedor");

                    if (objOPVenda.Entidade == null)
                    {

                        queryResult = PriEngine.Engine.Consulta(@"SELECT CabecOportunidadesVenda.Oportunidade, CabecOportunidadesVenda.Descricao, CabecOportunidadesVenda.Entidade, CabecOportunidadesVenda.TipoEntidade,CabecOportunidadesVenda.DataCriacao, Zonas.Descricao as zona, EntidadesExternas.Nome, EntidadesExternas.Email, EntidadesExternas.Morada, EntidadesExternas.Telemovel, EntidadesExternas.Entidade
                    FROM CabecOportunidadesVenda 
                    LEFT JOIN Zonas ON(CabecOportunidadesVenda.Zona = Zonas.Zona)
                    JOIN EntidadesExternas ON (CabecOportunidadesVenda.Entidade = EntidadesExternas.Entidade)
                    WHERE CabecOportunidadesVenda.Oportunidade = '" + codOpVenda + "'"
                        );


                        objOPVenda.OportunidadeID = queryResult.Valor("Oportunidade");
                        objOPVenda.DescricaoOp = queryResult.Valor("Descricao");
                        objOPVenda.Entidade = queryResult.Valor("entidade");
                        objOPVenda.TipoEntidade = queryResult.Valor("TipoEntidade");
                        objOPVenda.Data = queryResult.Valor("DataCriacao");
                        objOPVenda.Zona = queryResult.Valor("zona");
                        objOPVenda.Nome = queryResult.Valor("Nome");
                        objOPVenda.Email = queryResult.Valor("Email");
                        objOPVenda.Morada = queryResult.Valor("Morada");
                        objOPVenda.Telemovel = queryResult.Valor("Telemovel");
                        objOPVenda.VendedorCod = queryResult.Valor("Vendedor");

                    }
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



                StdBELista queryResult = PriEngine.Engine.Consulta(@"SELECT CabecOportunidadesVenda.Oportunidade, CabecOportunidadesVenda.Descricao, CabecOportunidadesVenda.DataCriacao, CabecOportunidadesVenda.TipoEntidade, CabecOportunidadesVenda.Entidade as entidade, CabecOportunidadesVenda.Vendedor, Zonas.Descricao as zona , Clientes.Nome, Clientes.Fac_Mor, Clientes.Fac_Tel
                    FROM CabecOportunidadesVenda 
                    LEFT JOIN Zonas ON(CabecOportunidadesVenda.Zona = Zonas.Zona)
                    JOIN Clientes ON (CabecOportunidadesVenda.Entidade = Clientes.Cliente)
                    
                ");

                while (!queryResult.NoFim())
                {
                    listOpVenda.Add(new Model.OportunidadeVenda
                    {
                        OportunidadeID = queryResult.Valor("Oportunidade"),
                        DescricaoOp = queryResult.Valor("Descricao"),
                        TipoEntidade = queryResult.Valor("TipoEntidade"),
                        Entidade = queryResult.Valor("entidade"),
                        VendedorCod = queryResult.Valor("Vendedor"),
                        Data = queryResult.Valor("DataCriacao"),
                        Zona = queryResult.Valor("zona"),
                        Nome = queryResult.Valor("Nome"),
                        Morada = queryResult.Valor("Fac_Mor"),
                        Telemovel = queryResult.Valor("Fac_Tel"),
                    });

                    queryResult.Seguinte();
                }

                queryResult = PriEngine.Engine.Consulta(@"SELECT CabecOportunidadesVenda.Oportunidade, CabecOportunidadesVenda.Descricao,CabecOportunidadesVenda.DataCriacao,  CabecOportunidadesVenda.TipoEntidade,CabecOportunidadesVenda.Entidade as entidade, CabecOportunidadesVenda.Vendedor, Zonas.Descricao as zona , EntidadesExternas.Nome, EntidadesExternas.Morada, EntidadesExternas.Telemovel
                    FROM CabecOportunidadesVenda 
                    LEFT JOIN Zonas ON(CabecOportunidadesVenda.Zona = Zonas.Zona)
                    JOIN EntidadesExternas ON (CabecOportunidadesVenda.Entidade = EntidadesExternas.Entidade)
                    
                ");
                   
                while (!queryResult.NoFim())
                {
                    listOpVenda.Add(new Model.OportunidadeVenda
                    {
                        OportunidadeID = queryResult.Valor("Oportunidade"),
                        DescricaoOp = queryResult.Valor("Descricao"),
                        TipoEntidade = queryResult.Valor("TipoEntidade"),
                        Entidade = queryResult.Valor("entidade"),
                        VendedorCod = queryResult.Valor("Vendedor"),
                        Data = queryResult.Valor("DataCriacao"),
                        Zona = queryResult.Valor("zona"),
                        Nome = queryResult.Valor("Nome"),
                        Morada = queryResult.Valor("Morada"),
                        Telemovel = queryResult.Valor("Telemovel")
                    });

                    queryResult.Seguinte();
                }

                return listOpVenda;

            }
            else
            {
                return null;

            }

        }

        #endregion OportunidadeVenda

    }
}
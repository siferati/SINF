# SINF

### Privamera Documentation

* [Primavera API Documentation](http://www.primaverabss.com/pkb/Homepage-Detalhes_Artigo2.aspx?SourceID=2d3b2a63-e518-458c-846d-9254cae91bbe&Level=3&ParentCat=d08ca5de-6975-4a3b-b1e9-072a3fe5f114&CatPath=L900%40ERP900%40d08ca5de-6975-4a3b-b1e9-072a3fe5f114&ItemKey=c9c925d3-d716-415a-8edc-fe5a2a036010)
* [Primavera Database Documentation](http://www.primaverabss.com/pkb/Homepage-Detalhes%20Categoria.aspx?SourceID=c2d14572-bdba-4a38-af6b-36b1ac4f8bc9&Level=3&ParentCat=TBL&CatPath=139b7747-6fcc-11de-9abd-00155d06082b%40c17dd489-af4c-11e3-a101-00155d0ece6a@TBL&ParentCategoryDescription=TBL)


### How to import Primavera configurations

* Open ``Primavera Admin``
* Click ``Empresas`` on the left side
* On the right side, Right Mouse Button on`` DEMOSINF`` > ``Administração`` > ``Importar/Exportar Configurações``
* Choose ``Importar definições de XML`` and select ``primavera.xml``
* Select ``Tabelas de Utilizador`` and ``Campos de Utilizador``

### How to run RESTful Service

* Open ``FirstREST`` with Visual Studio
* Right Mouse Button on ``References`` (right side, under solution explorer) > ``add Reference``
* Click ``Browse...`` > Type ``inter*`` and press ``Enter`` > Select them all and click ``Add`` > Click ``Ok``
* Press Run (Google Chrome) - it should open http://localhost:49822/ in browser


### RESTful Service Endpoints

#### Sales Representative

* http://localhost:49822/api/vendedores - GET all sales reps
* http://localhost:49822/api/vendedores/3 - GET given sales rep
* http://localhost:49822/api/vendedores/3/orders - GET sales of given rep
* http://localhost:49822/api/vendedores - POST new sales reps (fiscalId, name, address, phone, email, birthDate, hiredDate, description)
* http://localhost:49822/api/vendedores/3 - PUT update given sales rep (name, address, phone, email, birthDate, hiredDate, description)

#### Customer

* http://localhost:49822/api/clientes - GET all customers
* http://localhost:49822/api/clientes/SOLUCAO-Z - GET given customer
* http://localhost:49822/api/clientes/SOLUCAO-Z/orders - GET sales of given customer
* http://localhost:49822/api/clientes - POST new customer (fiscalId, name, address, phone, email, description)
* http://localhost:49822/api/clientes/SOLUCAO-Z - PUT update given customer (name, address, phone, email, description)

#### Sales Orders

* http://localhost:49822/api/orders/DBAE7851-AC30-11E6-A18F-080027397412 - GET given sales order

#### Opportunities

* http://localhost:49822/api/oportunidadeVenda - GET all sales oportunities
* http://localhost:49822/api/oportunidadeVenda/OPV001 - GET given sales oportunity
* http://localhost:49822/api/oportunidadeVenda - POST new oportunity (OportunidadeID, DescricaoOp, Entidade, Data, Local, VendedorCod)
* http://localhost:49822/api/oportunidadeVenda/OPV001 - PUT updated oportunity ( DescricaoOp, Entidade, Data, Local, VendedorCod)

#### Products

* http://localhost:49822/api/artigos - GET all products
* http://localhost:49822/api/artigos/a0001  - GET given product

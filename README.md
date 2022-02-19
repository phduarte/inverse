# Inverse DB
[![.NET Core Desktop](https://github.com/phduarte/inverse/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/phduarte/inverse/actions/workflows/dotnet-desktop.yml)
[![codecov](https://codecov.io/gh/phduarte/inverse/branch/master/graph/badge.svg?token=MmEDT29uzh)](https://codecov.io/gh/phduarte/inverse)

Sistema de Engenharia Reversa de Banco de Dados compatível com SQLite e SQL Server

## Uso

Escolha qual o tipo de provedor de banco de dados o banco está sendo executado:

![seleção de provedor](tela2.png)

Informe os dados de conexão, no caso abaixo, para uma conexão com SQL Server é necessário informar o servidor e o nome do banco de dados.

![informações da conexão](tela3.png)

Na tela principal do sistema é possivel mover as tabelas e organizar a visualização do diagrama. Após terminar é possível salvar o diagrama para continuar mais tarde.

![tela principal](tela1.png)

Também é possível exportar o diagrama para arquivo de script `.SQL`. Isso facilita por exemplo a recriação da estrutura do banco de dados em outro servidor ou ambiente.

![exportação de script](tela4.png)

## Requisitos:

- Plataforma Windows Forms .NET 5
- Linguagem C# versão 9.0
- xUnit 2.4.1+
- FakeItEasy 7.3+

# Inverse DB
[![.NET Core Desktop](https://github.com/phduarte/inverse/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/phduarte/inverse/actions/workflows/dotnet-desktop.yml)
[![codecov](https://codecov.io/gh/phduarte/inverse/branch/master/graph/badge.svg?token=MmEDT29uzh)](https://codecov.io/gh/phduarte/inverse)

Inverse DB is a reverse engineering tool for databases, supporting both SQLite and SQL Server. It enables users to connect to an existing database, visualize its schema as an interactive diagram, and export the structure for use in other environments.

## Features

- **Database Provider Selection:** Choose between SQLite and SQL Server as your data source.
- **Connection Management:** Enter connection details to access your database.
- **Schema Visualization:** View and organize tables in a graphical diagram, with drag-and-drop support for layout customization.
- **Diagram Persistence:** Save and reload your diagrams to continue work at any time.
- **Script Export:** Generate `.SQL` scripts from your diagrams to recreate the database structure elsewhere.

## Getting Started

1. **Select Provider:** Choose your database type (SQLite or SQL Server).
2. **Connect:** Enter the required connection information.
3. **Visualize:** Explore and arrange your database tables in the main diagram view.
4. **Save/Export:** Save your diagram for later or export it as a SQL script.

1. ## Usage

Escolha qual o tipo de provedor de banco de dados o banco está sendo executado:

![seleção de provedor](tela2.png)

Informe os dados de conexão, no caso abaixo, para uma conexão com SQL Server é necessário informar o servidor e o nome do banco de dados.

![informações da conexão](tela3.png)

Na tela principal do sistema é possivel mover as tabelas e organizar a visualização do diagrama. Após terminar é possível salvar o diagrama para continuar mais tarde.

![tela principal](tela1.png)

Também é possível exportar o diagrama para arquivo de script `.SQL`. Isso facilita por exemplo a recriação da estrutura do banco de dados em outro servidor ou ambiente.

![exportação de script](tela4.png)

## Requirements

- **Operating System:** Windows 7+ or Linux
- **Runtime:** .NET 9
- **Processor:** 1 GHz or faster
- **Memory:** 512 MB RAM

## Development

- **IDE:** Visual Studio 2022 (v17.0+) or VS Code
- **SDK:** Windows Forms .NET 9
- **Language:** C# 10.0
- **Testing:** xUnit 2.5.0+, FakeItEasy 7.4+

## License

This project is open source and available under the MIT License.

## Contributing

Contributions are welcome! Please submit issues or pull requests via [GitHub](https://github.com/phduarte/inverse).

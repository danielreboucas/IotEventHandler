# IoT Event Handler

Este projeto é uma API Web ASP.NET Core para receber e processar eventos de dispositivos IoT. O objetivo é expor um endpoint para simuladores e integrações enviarem dados como temperatura, umidade e identificador do dispositivo, persistindo essas informações em banco de dados e associando ao dispositivo correspondente.

---

## Funcionalidades

- Recebe dados de eventos via HTTP POST
- Valida o payload recebido
- Persiste os dados no banco de dados
- Associa eventos ao dispositivo correto via IntegrationId
- Extensível para outros tipos de dados de sensores

---

## Tecnologias Utilizadas

- **Linguagem:** C# (.NET 7 ou .NET 8)
- **Framework:** ASP.NET Core Web API
- **Banco de Dados:** SQL Server (padrão, mas pode ser configurado)
- **ORM:** Entity Framework Core
- **IDE:** Visual Studio 2022+ ou Visual Studio Code

---

## Requisitos

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- SQLite
- [Visual Studio 2022+](https://visualstudio.microsoft.com/vs/) ou [VS Code](https://code.visualstudio.com/)
- (Opcional) Docker para desenvolvimento containerizado

---

## Instalação e Execução

1. **Clone o repositório**
   ```bash
   git clone https://github.com/danielreboucas/IotEventHandler.git
   cd iot_event_handler
   ```

2. **Instale as dependências**
   ```bash
   dotnet restore
   ```

3. **Configure a string de conexão**

   Edite `appsettings.json`:
   ```json
     "ConnectionStrings": {
        "DefaultConnection": "Data Source=app.db"
      },
     "ExternalApiSettings": {
       "BaseUrl": "http://localhost:5000",
       "CallbackUrl": "http://localhost:5038/api"
      }
   ```

---

## Migrações de Banco de Dados

1. **Criar uma migration**
   ```bash
   dotnet ef migrations add InitialCreate --project iot_event_handler.Infrastructure
   ```

2. **Aplicar a migration**
   ```bash
   dotnet ef database update --project iot_event_handler.Infrastructure
   ```

   > Certifique-se que o SQLite está rodando.

---

## Uso da API

**Testes**

- Utilize o Swagger (`/swagger`) ou o Postman para testar os endpoints localmente.

---

## Desenvolvimento

- Abra o projeto com Visual Studio ou VS Code.
- Rode o projeto com `dotnet run` ou via IDE.
- Utilize o Swagger para explorar e testar a API.

## Porque Clean Architecture?
### 1. **Separação de Responsabilidades**
A Clean Architecture força uma separação clara entre camadas de domínio (regras de negócio), aplicação (casos de uso), infraestrutura (banco de dados, frameworks) e interfaces (controllers, APIs). Isso reduz acoplamento e facilita entender e modificar o sistema.

### 2. **Facilidade para Testes**
Como as regras de negócio ficam isoladas, é possível testar o domínio e os casos de uso sem depender do banco de dados ou de frameworks. Testes unitários se tornam mais simples e rápidos.

### 3. **Independência de Tecnologias**
A arquitetura permite trocar detalhes de implementação (ex: banco de dados, interface web, mensageria) sem afetar o núcleo do sistema. Isso proporciona maior flexibilidade para evoluir o projeto ou adaptar-se a novas demandas tecnológicas.

### 4. **Manutenção e Evolução Facilitadas**
Projetos de longa duração tendem a mudar requisitos e tecnologias. A Clean Architecture prepara o código para essas mudanças, evitando grandes refatorações e permitindo que novas funcionalidades sejam adicionadas de forma organizada.

### 5. **Reutilização e Portabilidade**
Os componentes centrais (domínio e casos de uso) podem ser reutilizados em diferentes contextos (ex: trocar a interface de API por uma interface gráfica, ou migrar para outro banco de dados) sem reescrever o núcleo da aplicação.

### 6. **Organização e Clareza**
O padrão determina convenções claras de organização dos arquivos e dependências, facilitando a entrada de novos desenvolvedores e a colaboração em equipe.


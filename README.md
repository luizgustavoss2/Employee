# Projeto de Cadastro de Funcionários

Este projeto consiste em um sistema de cadastro de funcionários, desenvolvido com um **Backoffice em .NET** e uma **interface frontend em Angular**. 
Ele permite o gerenciamento de funcionários, incluindo a adição de lista de telefones e permissões para cada usuário.

---

## Funcionalidades

- **Cadastro de Funcionários:**
  - Cadastre novos funcionários com informações como nome, e-mail, documento, data de nascimento, senha e gerente associado.
  - Adicione uma lista de telefones para cada funcionário.
  - Atribua permissões específicas a cada funcionário.

- **Login:**
  - Tela de login para autenticação de usuários.
  - Utilize o e-mail `admin@admin` e a senha `123` para acessar o sistema.

- **Listagem e Edição:**
  - Visualize a lista de funcionários cadastrados.
  - Edite ou exclua funcionários existentes.

---

## Tecnologias Utilizadas

- **Backend:**
  - .NET (Backoffice)
  - Swagger (Documentação da API)
  - Docker (Configuração do banco de dados)

- **Frontend:**
  - Angular (Interface do usuário)
  - NgxMask (Máscaras para campos de data e telefone)

- **Banco de Dados:**
  - Configurado via Docker Compose, base DB_FUNCIONARIO

---

## Como Executar o Projeto

### Pré-requisitos

- [Docker](https://www.docker.com/) (para o banco de dados)
- [.NET SDK](https://dotnet.microsoft.com/download) (para o backend)
- [Node.js](https://nodejs.org/) e [Angular CLI](https://angular.io/cli) (para o frontend)

---

### Passo 1: Configurar o Banco de Dados

1. Navegue até a pasta raiz do projeto (onde está o arquivo `docker-compose.yml`).
2. Execute o seguinte comando para subir o banco de dados:
   docker-compose up

   
### Passo 2: Executar o Backoffice (.NET)
1. Navegue até a pasta do backend (onde está o arquivo .sln do projeto .NET).

2. Execute o projeto .NET. Isso iniciará o Backoffice e disponibilizará a API.

3. Acesse a documentação da API no Swagger através da URL: https://localhost:5001/index.html

### Passo 3: Executar o Frontend (Angular)
1. Navegue até a pasta do frontend (projeto/front).

2. Instale as dependências do projeto:
	npm install
	
3. Inicie o servidor de desenvolvimento do Angular: 
    ng serve
	
4. Acesse a aplicação no navegador através da URL:
	http://localhost:4200
	
## Login Inicial
	Para começar a usar o sistema, utilize as seguintes credenciais:

	E-mail: admin@admin
	Senha: 123




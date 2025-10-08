# TechChallenge App Function

[Documentação completa do projeto](https://alealencarr.github.io/TechChallenge/)

Contém o código C# da Azure Function. O pipeline de CI/CD está configurado e faz o deploy deste código para o Azure.

### Descrição
Este repositório contém o código-fonte da nossa função serverless de autenticação. A sua responsabilidade é identificar o cliente (seja por CPF ou como anónimo) e gerar um token JWT correspondente.

Ela foi projetada para ser leve, escalável e ter uma responsabilidade única, desacoplando a lógica de autenticação da API principal.

### Tecnologias Utilizadas
.NET 8 (Isolated Worker): Framework da aplicação.

Azure Functions: Plataforma de execução serverless.

JWT (JSON Web Tokens): Para a geração dos tokens de autenticação.

### Responsabilidades
Implementar o endpoint POST /register para criar novos clientes na base de dados.

Implementar o endpoint GET /api/auth para:

Gerar um token JWT para um cliente identificado (se um CPF for fornecido e encontrado na base de dados).

Gerar um token JWT para um cliente anónimo (se nenhum CPF for fornecido).

Conectar-se à base de dados Azure SQL para consultar e registar clientes.

### Dependências
O pipeline de CI/CD deste repositório depende da existência da infraestrutura criada pelo TechChallenge-infra-compute (a Function App) e pelo TechChallenge-infra-data (a base de dados).

### Processo de CI/CD
O pipeline de CI/CD configurado neste repositório (.github/workflows/deploy.yml) é acionado a cada merge na branch main e executa os seguintes passos:

Build e Publish: Compila o código da função e gera os artefatos para deploy.

Deploy na Azure Function: Usa a action Azure/functions-action para publicar o código na Function App correspondente no Azure.

Configura as Application Settings: Injeta de forma segura as configurações necessárias (Connection String, chave do JWT) nos settings da aplicação no Azure.

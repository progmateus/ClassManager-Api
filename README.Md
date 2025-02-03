# Class Manager 
[![NPM](https://img.shields.io/npm/l/react)](https://github.com/devsuperior/sds1-wmazoni/blob/master/LICENSE) 

# Sobre o projeto

  Class Manager é um saas para ajudar as escolas esportivas a gerenciar seus alunos, aulas e pagamento da smensalidades

 Para a segurança da aplicação, foram limitadas todas as entradas do backend prevenindo Buffer overflow, foram validados todos os parametros utilizando Regex para evitar XSS e SQLI. E para evitar o envio de de arquivos maliciosos para a aplicação foi validado o input de avatar do usuário 

## Rodando aplicação localmente

1. Clonar o repositório:

   ```bash
    git clone https://github.com/progmateus/ClassManager-Api.git
   ```

2. Navegar até o diretório do projeto::

   ```bash
    cd ClassManager
   ```

3. Restaurar dependências

   ```bash
    dotnet restore
   ```

4. Rodar container docker

   ```bash
    docker compose up
   ```

5. Compilar o Projeto

   ```bash
    dotnet build
   ```


6. Rodar aplicação
   ```bash
    dotnet run
   ```


## Tecnologias utilizadas
- .NET Core
- ASP.NET Core
- Docker
- Entity Framework Core
- SQL Server
- Bulls
- RabbitMq
- Stripe
- AutoMapper


# Autor

Mateus Lopes

https://www.linkedin.com/in/progmateus
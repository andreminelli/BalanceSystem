# BalanceSystem

Implementação de um controle de fluxo de caixa diário, com lançamentos (débitos e créditos) e um relatório com saldo diário consolidado.

Principais tecnologias e frameworks utilizados:

* .Net 6.0
* ASP.NET Core 6.0
* Entity Framework 6.0
* PostgreSql 15
* JSON Web Tokens (JWT)
* OpenAPI (Swagger)

## Arquitetura de sistema

O diagrama de contexto de sistema abaixo dá uma visão de alto nível das interações e funcionalidades:

![Contexto de Sistema](/docs/system_context.png)

O diagrama de containers abaixo mostra uma visão um pouco mais detalhada do sistema.

![Containers](/docs/containers.png)

Ele foi implementado como uma API REST, usando ASP.NET Core. Os dados são salvos em um banco de dados relacional PostgreSql, utilizando Entity Framework Core 6.0 para a persistência.

A implantação completa prevê um API Gateway a fim de expor a API REST anterior, com as responsabilidades de:

 * gerir a base de usuários com acesso à API, gerando tokens JWT (utilizados para a autenticação da API)
	
 * fazer a terminação HTTPS para o cliente
	 
 * registrar informações de monitoramento (como métricas de tempo de resposta e quantidade de sucesso/falha)

> O API Gateway não faz parte deste repositório. Uma solução OSS como Kong poderia ser adicionada para exemplificar o sistema completo.

## Arquitetura de software

O código foi estruturado baseado em _onion architecture_, conforme a imagem abaixo:

![Estrutura de projetos](/docs/code_diagram.png)

## Como executar

Requisitos para execução:
* Docker & Docker Compose (configurado para usar *containers* Linux)
 
### Pelo Docker

Para iniciar, execute o comando abaixo a partir da raiz do repositório:
```
 docker compose -f ./src/docker-compose.yml up
```

> **Observação**: é necessário passar explicitamente o arquivo como acima para que o arquivo docker-compose.override.yml (usado apenas em tempo de desenvolvimento) não seja carregado.

### Pelo Visual Studio 

É recomendado utilizar o Visual Studio 2022. 

Abra a *solution* em `src\BalanceSystem.sln`. Configure a inicialização para utilizar o Docker Compose, conforme a figura abaixo:

![Estrutura de projetos](/docs/vs_start_docker_compose.jpg)

### Acessando a API

Está disponível no endereço base: 

```
http://localhost:9000
```

Interface de usuário OpenApi está disponivel em:

```
http://localhost:9000/swagger
```

### Geração do JWT para autenticação

A autenticação de usuários é feita utilizando tokens JWT. O *Subject* do token é tratado como o identificador do usuário no sistema. Abaixo temos um exemplo de _payload_ de um token JWT:

```json
{
    "iss": "An API Gateway",
    "iat": 1689784635,
    "exp": 1689785837,
    "aud": "balance-system",
    "sub": "2358022",
    "name": "Bob Doe"
}
```

No exemplo acima o sistema irá criar um usuário com identificador 2358022 e nome Bob Doe, utilizando este contexto para as operações na API.

Para testes, qualquer JWT gerado usando a chave `qwertyuiopasdfghjklzxcvbnm123456`. 

> Esta chave é configurada no arquivo `appsetting.json`, pelo array em `Jwt:Keys`.

Neste [site](http://jwtbuilder.jamiekurtz.com/) pode-se gerar JWT com expiração facilmente. 
A chave no projeto já está configurada para a mesma utilizada ali. 

Basta ajustar o *subject* conforme o exemplo acima, escolher a expiração do token e clicar no botão "Create Signed JWT", no final da página.

## Recomendações para ambiente de produção

Em um ambiente de produção real as seguintes melhorias são recomendadas:

* Definição efetiva do API Gateway a ser usado, capaz de atender as responsabilidades mencionadas anteriormente.

* Utilização de um banco de dados de segredos (como por exemplo Vault, da Hashicorp), a fim de armazenar de modo seguro:

	* credenciais de acesso ao banco de dados.

	* chaves de validação dos tokens JWT

* Implantação da API em ambiente escalável e resiliente, sendo Kubernetes muito indicado (inclusive por ser facilmente integrado, já que o sistema já se encontra 'conteinerizado'). 
 
* Exportação de dados de monitoramento (métricas e logs) integrados a uma solução de APM, para ter observabilidade mais robusta para a operação do sistema.

## Melhorias adicionais

Várias melhorias podem ser aplicadas ao sistema a fim de suportar cargas maiores, como por exemplo:

* Inclusão de cache usando Redis, para reduzir carga no banco de dados e melhorar o tempo de resposta da API

* Implementação da camada _Application Services_ usando CQRS, como forma de potencializar a otimização das operações de leitura/escrita de dados de individualizada

* Uso de replicação do banco de dados, separando e otimizando a leitura de dados em uma réplica específica.


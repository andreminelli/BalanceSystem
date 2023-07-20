# BalanceSystem

Implementa��o de um controle de fluxo de caixa di�rio, com lan�amentos (d�bitos e cr�ditos) e um relat�rio com saldo di�rio consolidado.

Principais tecnologias e frameworks utilizados:

* .Net 6.0
* ASP.NET Core 6.0
* Entity Framework 6.0
* PostgreSql 15
* JSON Web Tokens (JWT)
* OpenAPI (Swagger)

## Arquitetura de sistema

O diagrama de contexto de sistema abaixo d� uma vis�o de alto n�vel das intera��es e funcionalidades:

![Contexto de Sistema](/docs/system_context.png)

O diagrama de containers abaixo mostra uma vis�o um pouco mais detalhada do sistema.

![Containers](/docs/containers.png)

Ele foi implementado como uma API REST, usando ASP.NET Core. Os dados s�o salvos em um banco de dados relacional PostgreSql, utilizando Entity Framework Core 6.0 para a persist�ncia.

A implanta��o completa prev� um API Gateway a fim de expor a API REST anterior, com as responsabilidades de:

 * gerir a base de usu�rios com acesso � API, gerando tokens JWT (utilizados para a autentica��o da API)
	
 * fazer a termina��o HTTPS para o cliente
	 
 * registrar informa��es de monitoramento (como m�tricas de tempo de resposta e quantidade de sucesso/falha)

> O API Gateway n�o faz parte deste reposit�rio. Uma solu��o OSS como Kong poderia ser adicionada para exemplificar o sistema completo.

## Arquitetura de software

O c�digo foi estruturado baseado em _onion architecture_, conforme a imagem abaixo:

![Estrutura de projetos](/docs/code_diagram.png)

## Como executar

Requisitos para execu��o:
* Docker & Docker Compose (configurado para usar *containers* Linux)
 
### Pelo Docker

Para iniciar, execute o comando abaixo a partir da raiz do reposit�rio:
```
 docker compose -f ./src/docker-compose.yml up
```

> **Observa��o**: � necess�rio passar explicitamente o arquivo como acima para que o arquivo docker-compose.override.yml (usado apenas em tempo de desenvolvimento) n�o seja carregado.

### Pelo Visual Studio 

� recomendado utilizar o Visual Studio 2022. 

Abra a *solution* em `src\BalanceSystem.sln`. Configure a inicializa��o para utilizar o Docker Compose, conforme a figura abaixo:

![Estrutura de projetos](/docs/vs_start_docker_compose.jpg)

### Acessando a API

Est� dispon�vel no endere�o base: 

```
http://localhost:9000
```

Interface de usu�rio OpenApi est� disponivel em:

```
http://localhost:9000/swagger
```

### Gera��o do JWT para autentica��o

A autentica��o de usu�rios � feita utilizando tokens JWT. O *Subject* do token � tratado como o identificador do usu�rio no sistema. Abaixo temos um exemplo de _payload_ de um token JWT:

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

No exemplo acima o sistema ir� criar um usu�rio com identificador 2358022 e nome Bob Doe, utilizando este contexto para as opera��es na API.

Para testes, qualquer JWT gerado usando a chave `qwertyuiopasdfghjklzxcvbnm123456`. 

> Esta chave � configurada no arquivo `appsetting.json`, pelo array em `Jwt:Keys`.

Neste [site](http://jwtbuilder.jamiekurtz.com/) pode-se gerar JWT com expira��o facilmente. 
A chave no projeto j� est� configurada para a mesma utilizada ali. 

Basta ajustar o *subject* conforme o exemplo acima, escolher a expira��o do token e clicar no bot�o "Create Signed JWT", no final da p�gina.

## Recomenda��es para ambiente de produ��o

Em um ambiente de produ��o real as seguintes melhorias s�o recomendadas:

* Defini��o efetiva do API Gateway a ser usado, capaz de atender as responsabilidades mencionadas anteriormente.

* Utiliza��o de um banco de dados de segredos (como por exemplo Vault, da Hashicorp), a fim de armazenar de modo seguro:

	* credenciais de acesso ao banco de dados.

	* chaves de valida��o dos tokens JWT

* Implanta��o da API em ambiente escal�vel e resiliente, sendo Kubernetes muito indicado (inclusive por ser facilmente integrado, j� que o sistema j� se encontra 'conteinerizado'). 
 
* Exporta��o de dados de monitoramento (m�tricas e logs) integrados a uma solu��o de APM, para ter observabilidade mais robusta para a opera��o do sistema.

## Melhorias adicionais

V�rias melhorias podem ser aplicadas ao sistema a fim de suportar cargas maiores, como por exemplo:

* Inclus�o de cache usando Redis, para reduzir carga no banco de dados e melhorar o tempo de resposta da API

* Implementa��o da camada _Application Services_ usando CQRS, como forma de potencializar a otimiza��o das opera��es de leitura/escrita de dados de individualizada

* Uso de replica��o do banco de dados, separando e otimizando a leitura de dados em uma r�plica espec�fica.


# BalanceSystem

Implementação de um controle de fluxo de caixa diário, com lançamentos (débitos e créditos) e um relatório com saldo diário consolidado.

# Como executar

Requisitos:
* Docker & Docker Compose

Para iniciar vá até a pasta `src` e execute o comando:
```
docker compose -f docker-compose.yml up
```

**OBS: é necessário passar explicitamente o arquivo como acima para que o arquivo docker-compose.override.yml, usado apenas em tempo de desenvolvimento, não seja carregado.**


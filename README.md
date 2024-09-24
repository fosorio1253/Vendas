# 123Vendas - Sistema de API de Vendas

Este projeto implementa uma **API de Vendas** para a empresa **123Vendas**, utilizando princípios de **DDD (Domain-Driven Design)**. O sistema é dividido em três domínios principais: **Estoque**, **CRM (Cliente)** e **Vendas**. O objetivo deste projeto é desenvolver a API de vendas que será responsável pelo gerenciamento de transações de vendas, incluindo seus produtos e valores.

## Funcionalidades

A API permite o **CRUD completo** (Create, Read, Update, Delete) para registros de vendas, fornecendo as seguintes informações:

- **Número da venda**  
- **Data da venda**
- **Cliente** (com desnormalização do descritivo)
- **Valor total da venda**
- **Filial onde a venda foi efetuada**
- **Produtos**
- **Quantidades**
- **Valores unitários**
- **Descontos**
- **Valor total de cada item**
- **Status de cancelamento** (Cancelado/Não Cancelado)

### Publicação de Eventos (Diferencial)

Como diferencial, a API pode emitir eventos relacionados a mudanças no estado das vendas, tais como:

- `CompraCriada`
- `CompraAlterada`
- `CompraCancelada`
- `ItemCancelado`

Os eventos podem ser registrados no **log da aplicação** usando o **Serilog** para fins de simulação, mas sem a necessidade de uso de um message broker real (RabbitMQ ou Service Bus).

## Requisitos

Para o desenvolvimento desta API, as seguintes tecnologias, padrões e práticas devem ser utilizados:

### Padrões e Princípios
- **DDD (Domain-Driven Design)** com desnormalização de descrições de entidades externas
- **Git Flow workflow**
- **Commit semântico**
- **APIs REST**
- **Clean Code**
- **Princípios SOLID**
- **Princípio DRY (Don't Repeat Yourself)**
- **Princípio YAGNI (You Aren't Gonna Need It)**
- **Object Calisthenics**

### Logs
- **Serilog** para controle e registro de logs na aplicação

### Camadas
O projeto deve ser dividido nas seguintes camadas:
- **API**: Interface de entrada e saída do sistema
- **Domain**: Regras de negócio e entidades
- **Data**: Acesso a dados

### Testes
Testes de unidade devem ser implementados com:
- **XUnit**
- **FluentAssertions**
- **Bogus** (para geração de dados fake)
- **NSubstitute** (para mocks e stubs)

Testes de integração são desejáveis, usando:
- **Test Containers**

## Estrutura de Pastas

```plaintext
src/
 ├── Api/             # Implementação da camada de API
 ├── Domain/          # Implementação da lógica de domínio e entidades
 └── Data/            # Implementação do acesso aos dados
tests/
 ├── UnitTests/       # Testes unitários usando XUnit, FluentAssertions, Bogus, NSubstitute
 └── IntegrationTests/ # Testes de integração com Test Container

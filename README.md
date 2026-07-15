# Controle de Gastos Residenciais

Aplicação full stack desenvolvida para gerenciamento de gastos residenciais, permitindo o cadastro de pessoas, lançamento de receitas e despesas e consulta de totais individuais e gerais.

O projeto foi desenvolvido utilizando .NET 8 no back-end, React com TypeScript no front-end e SQLite para persistência dos dados.

---

## Tecnologias utilizadas

### Back-end

- .NET 8
- ASP.NET Core Minimal API
- Entity Framework Core
- SQLite

### Front-end

- React 18
- TypeScript
- Vite

### Testes

- xUnit
- SQLite em memória

---

## Funcionalidades

### Pessoas

- Cadastro de pessoas
- Listagem de pessoas
- Exclusão de pessoas
- Exclusão automática das transações vinculadas (Cascade Delete)

Cada pessoa possui:

- Identificador (GUID)
- Nome
- Idade

---

### Transações

- Cadastro de receitas
- Cadastro de despesas
- Listagem de transações

Cada transação possui:

- Identificador (GUID)
- Descrição
- Valor
- Tipo (Receita ou Despesa)
- Pessoa vinculada

---

### Consulta de totais

O sistema apresenta:

- Total de receitas por pessoa
- Total de despesas por pessoa
- Saldo por pessoa

Além disso, exibe:

- Total geral de receitas
- Total geral de despesas
- Saldo líquido geral

---

## Regras de negócio implementadas

- Os identificadores são gerados automaticamente utilizando GUID.
- Não é possível cadastrar uma transação para uma pessoa inexistente.
- Pessoas menores de 18 anos podem cadastrar apenas despesas.
- Ao excluir uma pessoa, todas as suas transações são removidas automaticamente.
- Os valores informados devem ser positivos.
- O saldo é calculado pela diferença entre receitas e despesas.
- Os dados permanecem salvos após o encerramento da aplicação através do banco SQLite.

---

## Estrutura do projeto

```
controle-gastos-residenciais
│
├── backend
│   ├── ControleGastos.Api
│   └── ControleGastos.Tests
│
└── frontend
```

---

## Como executar o projeto

### Pré-requisitos

- .NET SDK 8
- Node.js 20 ou superior

### Back-end

```bash
cd backend
dotnet restore
dotnet run
```

A API será iniciada em:

```
http://localhost:5080
```

---

### Front-end

```bash
cd frontend
npm install
npm run dev
```

Abra no navegador o endereço informado pelo Vite (normalmente):

```
http://localhost:5173
```

---

## Executando os testes

Na pasta **backend** execute:

```bash
dotnet test
```

---

## Endpoints

### Pessoas

| Método | Endpoint | Descrição |
|---------|----------|-----------|
| GET | /api/pessoas | Lista todas as pessoas |
| POST | /api/pessoas | Cadastra uma pessoa |
| DELETE | /api/pessoas/{id} | Remove uma pessoa e suas transações |

### Transações

| Método | Endpoint | Descrição |
|---------|----------|-----------|
| GET | /api/transacoes | Lista todas as transações |
| POST | /api/transacoes | Cadastra uma transação |

### Totais

| Método | Endpoint | Descrição |
|---------|----------|-----------|
| GET | /api/totais | Retorna os totais por pessoa e o total geral |

---

## Persistência

Os dados são armazenados em SQLite no arquivo:

```
backend/data/gastos.db
```

Dessa forma, as informações permanecem disponíveis mesmo após o encerramento da aplicação.

---

## Observações

O projeto foi desenvolvido seguindo os requisitos propostos no desafio técnico, priorizando simplicidade, organização do código e separação entre as responsabilidades do back-end e do front-end.

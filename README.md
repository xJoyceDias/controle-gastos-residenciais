# Controle de Gastos Residenciais

Aplicação full stack para cadastro de pessoas, lançamentos financeiros e consulta de totais por pessoa e geral.

## Tecnologias

- Back-end: .NET 8, ASP.NET Core Minimal API, Entity Framework Core e SQLite.
- Front-end: React 18, TypeScript e Vite.
- Testes: xUnit com banco SQLite em memória.

O SQLite grava os dados em `backend/data/gastos.db`; por isso os cadastros permanecem após encerrar a aplicação.

## Como executar

Pré-requisitos: [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0) e Node.js 20+.

Em dois terminais, a partir da raiz do repositório:

```bash
cd backend
dotnet restore
dotnet run
```

```bash
cd frontend
npm install
npm run dev
```

Abra o endereço mostrado pelo Vite (normalmente `http://localhost:5173`). A API fica em `http://localhost:5080`.

Para executar os testes de regras de negócio:

```bash
cd backend
dotnet test
```

## Regras de negócio implementadas

- O identificador de pessoas e transações é um GUID, gerado automaticamente no servidor.
- Não é possível criar uma transação para pessoa inexistente.
- Pessoas com menos de 18 anos só podem receber lançamentos de `Despesa`.
- Ao remover uma pessoa, suas transações são removidas junto (relacionamento com *cascade delete*).
- Os totais retornam receitas, despesas e saldo (`receitas - despesas`) para cada pessoa e para o conjunto.
- Valores são positivos; o tipo da transação define se soma receita ou despesa.

## Estrutura

```
backend/                  API, domínio, persistência e testes
frontend/                 interface React/TypeScript
```

## Endpoints

| Método | Rota | Finalidade |
| --- | --- | --- |
| GET | `/api/pessoas` | Lista pessoas |
| POST | `/api/pessoas` | Cria uma pessoa |
| DELETE | `/api/pessoas/{id}` | Remove pessoa e suas transações |
| GET | `/api/transacoes` | Lista transações |
| POST | `/api/transacoes` | Cria uma transação |
| GET | `/api/totais` | Consulta totais por pessoa e geral |

## Publicação no GitHub

```bash
git init
git add .
git commit -m "feat: controle de gastos residenciais"
git branch -M main
git remote add origin https://github.com/SEU_USUARIO/controle-gastos-residenciais.git
git push -u origin main
```

Crie o repositório como **público** no GitHub antes do `push`.

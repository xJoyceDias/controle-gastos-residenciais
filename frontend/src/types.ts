export type TipoTransacao = 'Despesa' | 'Receita';
export interface Pessoa { id: string; nome: string; idade: number }
export interface Transacao { id: string; descricao: string; valor: number; tipo: TipoTransacao; pessoaId: string; pessoaNome: string }
export interface TotalPessoa { pessoaId: string; nome: string; receitas: number; despesas: number; saldo: number }
export interface Totais { pessoas: TotalPessoa[]; totalReceitas: number; totalDespesas: number; saldoLiquido: number }

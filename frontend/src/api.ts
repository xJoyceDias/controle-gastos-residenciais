import type { Pessoa, Totais, Transacao, TipoTransacao } from './types';
const base = 'http://localhost:5080/api';
async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const response = await fetch(`${base}${path}`, { headers: { 'Content-Type': 'application/json' }, ...options });
  if (!response.ok) { const body = await response.json().catch(() => null); throw new Error(body?.mensagem ?? 'Não foi possível concluir a operação.'); }
  return response.status === 204 ? undefined as T : response.json();
}
export const api = {
  listarPessoas: () => request<Pessoa[]>('/pessoas'),
  criarPessoa: (nome: string, idade: number) => request<Pessoa>('/pessoas', { method: 'POST', body: JSON.stringify({ nome, idade }) }),
  excluirPessoa: (id: string) => request<void>(`/pessoas/${id}`, { method: 'DELETE' }),
  listarTransacoes: () => request<Transacao[]>('/transacoes'),
  criarTransacao: (descricao: string, valor: number, tipo: TipoTransacao, pessoaId: string) => request<Transacao>('/transacoes', { method: 'POST', body: JSON.stringify({ descricao, valor, tipo, pessoaId }) }),
  totais: () => request<Totais>('/totais')
};

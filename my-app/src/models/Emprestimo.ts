export interface Emprestimo {
  id: number; // Identificador único do empréstimo
  livroId: number; // Identificador do livro emprestado
  clienteId: number; // Identificador do cliente que pegou o livro
  dataDeEmprestimo: string; // Data de empréstimo no formato "YYYY-MM-DD"
  dataDeDevolucao: string | null; // Data de devolução, opcional
}

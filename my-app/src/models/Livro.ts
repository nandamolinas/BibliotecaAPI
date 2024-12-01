import { Cliente } from "./Cliente";

export interface Livro {
  livroId: string; // Chave primária para o livro
  titulo: string; // Título do livro
  autor: string; // Autor do livro
  anoDePublicacao: number; // Ano de publicação do livro
  ClienteId?: number; // Relacionamento com o Cliente (opcional)
  Cliente?: Cliente; // Referência ao Cliente que emprestou o livro
  DataDeEmprestimo?: string; // Data de empréstimo do livro (opcional)
  DataDeDevolucao?: string; // Data de devolução do livro (opcional)
}

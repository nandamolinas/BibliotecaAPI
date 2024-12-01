
import { Cliente } from "./Cliente";

export interface Livro {
  livroId?: string;
  titulo: string; // Alterado de 'nome' para 'titulo' para maior clareza
  autor: string; // Adicionado o autor para complementar a entidade
  anoDePublicacao?: number; // Propriedade opcional
  DataDeEmprestimo? : string;
  DataDeDevolucao? : string;
  ClienteId? : number;
  Cliente? : Cliente;
}

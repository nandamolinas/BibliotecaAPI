import { useEffect, useState } from "react";
import { Livro } from "../../models/Livro";
import axios from "axios";

// Função para formatar datas no formato DD/MM/YYYY
const formatarData = (data: string | undefined) => {
  if (!data) return "Não devolvido"; // Caso a data não exista, retorna uma mensagem padrão
  const date = new Date(data);
  return date.toLocaleDateString("pt-BR"); // Formata a data no padrão brasileiro
};

function ListarLivros() {
  const [livros, setLivros] = useState<Livro[]>([]);

  useEffect(() => {
    consultarLivros();
  }, []);

  // Função para consultar os livros usando axios
  function consultarLivros() {
    axios
      .get("http://localhost:5200/api/livros/listar")
      .then((resposta) => {
        setLivros(resposta.data);
        console.table(resposta.data); // Depuração para verificar a resposta
      })
      .catch((erro) => console.error("Erro ao buscar livros:", erro));
  }

  // Função para deletar um livro
  function deletar(id: number) {
    axios
      .delete(`http://localhost:5200/api/livros/deletar/${id}`)
      .then((resposta) => {
        console.log("Livro deletado com sucesso:", resposta.data);
        consultarLivros(); // Atualiza a lista de livros após deletar
      })
      .catch((erro) => console.error("Erro ao deletar livro:", erro));
  }

  // Função para devolver o livro
  function devolverLivro(id: number) {
    axios
      .post(`http://localhost:5200/api/livros/devolver/${id}`)
      .then((resposta) => {
        console.log("Livro devolvido com sucesso:", resposta.data);
        consultarLivros(); // Atualiza a lista de livros após devolver
      })
      .catch((erro) => console.error("Erro ao devolver livro:", erro));
  }

  return (
    <div id="ListarLivros" className="container">
      <h1>Listar Livros</h1>
      <table border={1}>
        <thead>
          <tr>
            <th>#</th>
            <th>Título</th>
            <th>Autor</th>
            <th>Ano de Publicação</th>
            <th>Data de Empréstimo</th>
            <th>Data de Devolução</th>
            <th>Cliente Emprestado</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {livros.length === 0 ? (
            <tr>
              <td colSpan={8} style={{ textAlign: "center" }}>
                Nenhum livro encontrado.
              </td>
            </tr>
          ) : (
            livros.map((livro) => (
              <tr key={livro.livroId}>
                <td>{livro.livroId}</td>
                <td>{livro.titulo}</td>
                <td>{livro.autor}</td>
                <td>{livro.anoDePublicacao}</td>
                <td>{formatarData(livro.DataDeEmprestimo)}</td>
                <td>{formatarData(livro.DataDeDevolucao)}</td>
                <td>{livro.Cliente ? livro.Cliente.nome : "Não emprestado"}</td>
                <td>
                  {livro.DataDeDevolucao ? (
                   <button onClick={() => devolverLivro(Number(livro.livroId))}>
                   Devolver
                 </button>                 
                  ) : (
                    <button onClick={() => deletar(Number(livro.livroId))}>
                      Deletar
                    </button>
                  )}
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
}

export default ListarLivros;

import { Link } from "react-router-dom";
import { useEffect, useState } from "react";
import { Livro } from "../../models/Livro";
import axios from "axios";

function ListarLivros() {
  const [livros, setLivros] = useState<Livro[]>([]);

  useEffect(() => {
    consultarLivros();
  }, []);

  function consultarLivros() {
    fetch("http://localhost:5200/api/livros/listar")
      .then((resposta) => {
        if (!resposta.ok) {
          throw new Error(`Erro na API: ${resposta.status} - ${resposta.statusText}`);
        }
        return resposta.text(); // Ler a resposta como texto
      })
      .then((texto) => {
        if (!texto) {
          console.warn("Resposta vazia da API.");
          return []; // Retorna uma lista vazia se a resposta for vazia
        }
        return JSON.parse(texto); // Converte para JSON apenas se houver texto
      })
      .then((livros) => {
        setLivros(livros);
        console.table(livros);
      })
      .catch((erro) => console.error("Erro ao buscar livros:", erro));
  }

  function deletar(id: string) {
    axios
      .delete(`http://localhost:5200/api/livros/deletar/${id}`)
      .then((resposta) => {
        console.log("Livro deletado com sucesso:", resposta.data);
        consultarLivros(); // Atualiza a lista de livros após deletar
      })
      .catch((erro) => console.error("Erro ao deletar livro:", erro));
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
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {livros.length === 0 ? (
            <tr>
              <td colSpan={7} style={{ textAlign: "center" }}>
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
                <td>{livro.DataDeEmprestimo}</td>
                <td>{livro.DataDeDevolucao}</td>
                <td>
                  <button onClick={() => deletar(livro.livroId!)}>Deletar</button>
                </td>
                {/* <td>
                  <Link to={`/pages/livro/alterar/${livro.id}`}>
                    Alterar
                  </Link>
                </td> */}
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
}

export default ListarLivros;

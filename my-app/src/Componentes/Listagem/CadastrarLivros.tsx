import { useEffect, useState } from "react";

function CadatrarLivros() {
    const [titulo, setTitulo] = useState("");
    const [autor, setAutor] = useState("");
    const [anoDePublicacao, setAnoDePublicacao] = useState("");
    const [DataDeEmprestimo, setDataDeEmprestimo] = useState("");
    const [DataDeDevolucao, setDataDeDevolucao] = useState("");
    const [ClienteId, setClienteId] = useState(0);
    const [clientes, setClientes] = useState<any[]>([]);
  
    useEffect(() => {
      fetch("http://localhost:5117/api/cliente/listar")
        .then((resposta) => {
          if (!resposta.ok) {
            throw new Error(`Erro ao carregar clientes: ${resposta.status}`);
          }
          return resposta.json();
        })
        .then((clientes) => setClientes(clientes))
        .catch((erro) => console.error("Erro ao buscar clientes:", erro));
    }, []);
  
    function enviarLivro(e: React.FormEvent) {
      e.preventDefault();
  
      const livro = {
        titulo: titulo,
        autor: autor,
        anoDePublicacao: anoDePublicacao ? parseInt(anoDePublicacao, 10) : undefined,
        DataDeEmprestimo: DataDeEmprestimo || undefined,
        DataDeDevolucao: DataDeDevolucao || undefined,
        ClienteId: ClienteId || undefined,
      };
  
      console.log("Enviando livro:", livro); // Depuração: verifica os dados antes do envio
  
      fetch("http://localhost:5200/api/livros/cadastrar", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(livro),
      })
        .then((resposta) => {
          if (!resposta.ok) {
            throw new Error(`Erro ao cadastrar livro: ${resposta.status}`);
          }
          return resposta.json();
        })
        .then((dados) => {
          console.log("Livro cadastrado com sucesso:", dados);
          alert("Livro cadastrado com sucesso!");
        })
        .catch((erro) => {
          console.error("Erro ao cadastrar livro:", erro);
          alert("Erro ao cadastrar livro. Confira o console para mais detalhes.");
        });
    }
  
    return (
      <div id="cadastro-livro">
        <h1>Cadastrar Livro</h1>
        <form onSubmit={enviarLivro}>
          <div>
            <label htmlFor="nome">Título</label>
            <input
              type="text"
              id="nome"
              name="nome"
              required
              placeholder="Digite o título do livro"
              value={titulo}
              onChange={(e) => setTitulo(e.target.value)}
            />
          </div>
          <div>
            <label htmlFor="descricao">Autor</label>
            <textarea
              id="descricao"
              name="descricao"
              required
              placeholder="Digite o nome do autor"
              value={autor}
              onChange={(e) => setAutor(e.target.value)}
            ></textarea>
          </div>
          <div>
            <label htmlFor="quantidade">Ano de Publicação</label>
            <input
              type="number"
              id="quantidade"
              name="quantidade"
              placeholder="Digite o ano de publicação"
              value={anoDePublicacao}
              onChange={(e) => setAnoDePublicacao(e.target.value)}
            />
          </div>
          <div>
            <label htmlFor="cliente">Cliente</label>
            <select
              id="cliente"
              value={ClienteId}
              onChange={(e) => setClienteId(Number(e.target.value))}
            >
              <option value={0}>Selecione um cliente</option>
              {clientes.map((cliente) => (
                <option key={cliente.id} value={cliente.id}>
                  {cliente.nome}
                </option>
              ))}
            </select>
          </div>
          <div>
            <label htmlFor="emprestimo">Data de Empréstimo</label>
            <input
              type="date"
              id="emprestimo"
              name="emprestimo"
              value={DataDeEmprestimo}
              onChange={(e) => setDataDeEmprestimo(e.target.value)}
            />
          </div>
          <div>
            <label htmlFor="devolucao">Data de Devolução</label>
            <input
              type="date"
              id="devolucao"
              name="devolucao"
              value={DataDeDevolucao}
              onChange={(e) => setDataDeDevolucao(e.target.value)}
            />
          </div>
          <div>
            <button type="submit">Cadastrar Livro</button>
          </div>
        </form>
      </div>
    );
  }
  
  export default CadatrarLivros;
  
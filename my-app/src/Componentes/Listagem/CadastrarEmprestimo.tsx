import React, { useEffect, useState } from "react";
import axios, { AxiosError } from "axios";

const CadastrarEmprestimo = () => {
  const [livros, setLivros] = useState<any[]>([]);
  const [clientes, setClientes] = useState<any[]>([]);
  const [livroId, setLivroId] = useState<string | undefined>(undefined);
  const [clienteId, setClienteId] = useState<number | undefined>(undefined);
  const [dataDeEmprestimo, setDataDeEmprestimo] = useState<string>("");
  const [dataDeDevolucao, setDataDeDevolucao] = useState<string | null>(null);
  const [mensagem, setMensagem] = useState<string>("");

  // Carregar livros e clientes ao montar o componente
  useEffect(() => {
    const carregarDados = async () => {
      try {
        const [livrosResponse, clientesResponse] = await Promise.all([
          axios.get("http://localhost:5200/api/livros/listar"),
          axios.get("http://localhost:5200/api/clientes/listar"),
        ]);
        setLivros(livrosResponse.data);
        setClientes(clientesResponse.data);
      } catch (error) {
        console.error("Erro ao carregar dados:", error);
        setMensagem("Erro ao carregar livros ou clientes.");
      }
    };
    carregarDados();
  }, []);

  // Função para lidar com o envio do formulário
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    // Verificar se todos os campos obrigatórios foram preenchidos (livroId, clienteId e dataDeEmprestimo)
    if (!livroId || !clienteId || !dataDeEmprestimo) {
      setMensagem("Por favor, preencha todos os campos obrigatórios.");
      console.log("Erro nos campos:", { livroId, clienteId, dataDeEmprestimo }); // Log de depuração
      return;
    }

    // Montar o objeto do empréstimo
    const novoEmprestimo = {
      livroId: livroId, // Livro selecionado
      clienteId: clienteId, // Cliente selecionado
      dataDeEmprestimo: dataDeEmprestimo, // Data de empréstimo
      dataDeDevolucao: dataDeDevolucao ? dataDeDevolucao : null, // Se não houver data de devolução, envia null
    };

    console.log("Enviando empréstimo:", novoEmprestimo); // Log de depuração

    try {
      // Enviar o empréstimo para o backend
      const response = await axios.post("http://localhost:5200/api/emprestimos/cadastrar", novoEmprestimo);
      console.log("Resposta do backend:", response); // Log da resposta

      if (response.status === 200) {
        // Exibir mensagem de sucesso
        setMensagem("Empréstimo cadastrado com sucesso!");

        // Limpar campos após o envio
        setLivroId(undefined); // Limpa o ID do livro
        setClienteId(undefined); // Limpa o ID do cliente
        setDataDeEmprestimo(""); // Limpa a data de empréstimo
        setDataDeDevolucao(null); // Limpa a data de devolução
      } else {
        setMensagem("Erro ao cadastrar empréstimo. Tente novamente.");
      }
    } catch (error) {
      // Acha que é um erro do tipo AxiosError
      if (axios.isAxiosError(error)) {
        console.error("Erro ao cadastrar empréstimo:", error.response || error.message); // Exibe o erro corretamente
      } else {
        console.error("Erro desconhecido:", error); // Exibe erro genérico
      }
      setMensagem("Erro ao cadastrar empréstimo. Tente novamente.");
    }
  };

  return (
    <div>
      <h1>Cadastrar Empréstimo</h1>
      {mensagem && <p>{mensagem}</p>} {/* Exibe a mensagem de sucesso ou erro */}
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="livroId">Livro</label>
          <select
            id="livroId"
            value={livroId || ""}
            onChange={(e) => setLivroId(e.target.value)} // Garantir que o valor seja string
            required
          >
            <option value="" disabled>
              Selecione um livro
            </option>
            {livros.map((livro: any) => (
              <option key={livro.livroId} value={livro.livroId}>
                {livro.titulo}
              </option>
            ))}
          </select>
        </div>

        <div>
          <label htmlFor="clienteId">Cliente</label>
          <select
            id="clienteId"
            value={clienteId || ""}
            onChange={(e) => setClienteId(Number(e.target.value))} // Garantir que o valor seja número
            required
          >
            <option value="" disabled>
              Selecione um cliente
            </option>
            {clientes.map((cliente: any) => (
              <option key={cliente.clienteId} value={cliente.clienteId}>
                {cliente.nome}
              </option>
            ))}
          </select>
        </div>

        <div>
          <label htmlFor="dataDeEmprestimo">Data de Empréstimo</label>
          <input
            type="date"
            id="dataDeEmprestimo"
            value={dataDeEmprestimo || ""}
            onChange={(e) => setDataDeEmprestimo(e.target.value)}
            required
          />
        </div>

        <div>
          <label htmlFor="dataDeDevolucao">Data de Devolução (opcional)</label>
          <input
            type="date"
            id="dataDeDevolucao"
            value={dataDeDevolucao || ""}
            onChange={(e) => setDataDeDevolucao(e.target.value || null)} // Permite que a data de devolução seja null
          />
        </div>

        <div>
          <button type="submit">Cadastrar Empréstimo</button>
        </div>
      </form>
    </div>
  );
};

export default CadastrarEmprestimo;

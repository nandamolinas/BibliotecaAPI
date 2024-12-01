import React, { useState, useEffect } from "react";

// Definindo o tipo para os dados do empréstimo
interface Emprestimo {
  id: number;
  livroTitulo: string;
  clienteNome: string;
  dataDeEmprestimo: string;
  dataDeDevolucao: string | null;
}

const DevolucaoEmprestimo = () => {
  const [emprestimos, setEmprestimos] = useState<Emprestimo[]>([]); // Estado para armazenar os empréstimos
  const [emprestimoId, setEmprestimoId] = useState<number>(0); // Estado para armazenar o ID do empréstimo a ser devolvido
  const [mensagem, setMensagem] = useState<string>(""); // Estado para mensagens de sucesso ou erro

  // Carregar os empréstimos não devolvidos ao montar o componente
  useEffect(() => {
    const carregarEmprestimosNaoDevolvidos = async () => {
      try {
        const response = await fetch("http://localhost:5200/api/emprestimos/listar");
        if (response.ok) {
          const emprestimosData: Emprestimo[] = await response.json();
          // Filtra os empréstimos que não foram devolvidos
          const emprestimosNaoDevolvidos = emprestimosData.filter((emprestimo) => emprestimo.dataDeDevolucao === null);
          setEmprestimos(emprestimosNaoDevolvidos);
        } else {
          setMensagem("Erro ao carregar os empréstimos.");
        }
      } catch (error) {
        setMensagem("Erro ao carregar os empréstimos.");
      }
    };
    carregarEmprestimosNaoDevolvidos();
  }, []);

  // Função para lidar com a devolução
  const devolverEmprestimo = async () => {
    if (emprestimoId <= 0) {
      setMensagem("Por favor, selecione um empréstimo.");
      return;
    }

    const payload = { Id: emprestimoId };

    try {
      const response = await fetch("http://localhost:5200/api/emprestimos/devolver", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(payload),
      });

      if (response.ok) {
        setMensagem("Empréstimo devolvido com sucesso!");
        // Atualiza a lista de empréstimos não devolvidos após a devolução
        setEmprestimos(emprestimos.filter((emprestimo) => emprestimo.id !== emprestimoId));
      } else {
        const errorData = await response.json();
        setMensagem(errorData.message || "Erro ao devolver o empréstimo.");
      }
    } catch (error) {
      setMensagem("Erro desconhecido.");
    }
  };

  return (
    <div>
      <h2>Devolver Empréstimo</h2>

      {/* Exibir empréstimos não devolvidos */}
      <div>
        <label htmlFor="emprestimoId">Selecione um Empréstimo:</label>
        <select
          id="emprestimoId"
          value={emprestimoId}
          onChange={(e) => setEmprestimoId(Number(e.target.value))}
        >
          <option value={0} disabled>Selecione um empréstimo</option>
          {emprestimos.map((emprestimo) => (
            <option key={emprestimo.id} value={emprestimo.id}>
              ID: {emprestimo.id} - {emprestimo.livroTitulo} - {emprestimo.clienteNome}
            </option>
          ))}
        </select>
      </div>

      <button onClick={devolverEmprestimo}>Devolver</button>

      {/* Exibição de mensagens */}
      {mensagem && <p>{mensagem}</p>}
    </div>
  );
};

export default DevolucaoEmprestimo;

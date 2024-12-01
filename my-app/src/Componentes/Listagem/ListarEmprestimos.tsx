// src/Componentes/Listagem/ListarEmprestimos.tsx

import React, { useEffect, useState } from "react";
import axios from "axios";

interface Emprestimo {
  id: number;
  clienteId: number;
  livroId: number;
  dataDeEmprestimo: string;
  dataDeDevolucao?: string;
}

function ListarEmprestimos() {
  const [emprestimos, setEmprestimos] = useState<Emprestimo[]>([]);

  useEffect(() => {
    consultarEmprestimos();
  }, []);

  // Função para consultar os empréstimos
  function consultarEmprestimos() {
    axios
      .get("http://localhost:5200/api/emprestimos/listar") // Altere para a URL correta da sua API
      .then((response) => {
        setEmprestimos(response.data);
        console.table(response.data);
      })
      .catch((error) => {
        console.error("Erro ao carregar empréstimos:", error);
      });
  }

  return (
    <div>
      <h1>Listar Empréstimos</h1>
      <table border={1}>
        <thead>
          <tr>
            <th>#</th>
            <th>Cliente ID</th>
            <th>Livro ID</th>
            <th>Data de Empréstimo</th>
            <th>Data de Devolução</th>
          </tr>
        </thead>
        <tbody>
          {emprestimos.map((emprestimo) => (
            <tr key={emprestimo.id}>
              <td>{emprestimo.id}</td>
              <td>{emprestimo.clienteId}</td>
              <td>{emprestimo.livroId}</td>
              <td>{emprestimo.dataDeEmprestimo}</td>
              <td>{emprestimo.dataDeDevolucao ?? "Não devolvido"}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default ListarEmprestimos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.IO;

namespace ProcessamentoArquivosBaseMensal
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            grupoMensagemAlerta.Attributes["class"] = "input-group mensagemAlerta invisivel";

        }


        public void ImportarArquivo(object sender, EventArgs e)
        {
            {
                // Configurações do banco de dados
                string connectionString = @"Data Source=10.123.18.110; Initial Catalog = qldDB001; User ID=qldUD_001; Password=qldud001@#$";


                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Caminhos dos diretórios
                    string caminho = @"\\mg7009sr002\sistemas$\arquivo\2_copia_seguranca\projeto_operações_integradas\Produção Tematica";
                    string caminhoBackup = @"\\mg7009sr002\sistemas$\arquivo\2_copia_seguranca\projeto_operações_integradas\Produção Tematica";

               

                    if (Directory.Exists(caminho))
                    {
                        int vQtdArquivos = 0;
                        bool importei = false;

                        DirectoryInfo pasta = new DirectoryInfo(caminho);

                        foreach (FileInfo arquivo in pasta.GetFiles())
                        {
                            vQtdArquivos++;
                            string delim = ";";

                            string nomeArquivo = arquivo.Name;                  // Nome do arquivo
                            string enderecoCompleto = arquivo.FullName;          // Caminho completo do arquivo
                            long tamanhoArquivo = arquivo.Length;               // Tamanho do arquivo (em bytes)
                            DateTime dataDisponibilizacao = arquivo.CreationTimeUtc; // Data e hora da disponibilização
                            dataDisponibilizacao = Convert.ToDateTime(dataDisponibilizacao);

                            // Verifica se o arquivo já existe no banco de dados
                            string vSQLnomearq = $"SELECT no_arquivo FROM prTb001_Arquivo WHERE no_arquivo='{arquivo.Name}'";

                            SqlCommand cmdArquivo = new SqlCommand(vSQLnomearq, conn);
                            SqlDataReader rsArquivo = cmdArquivo.ExecuteReader();

                            bool arquivoExiste = rsArquivo.HasRows;
                            rsArquivo.Close();  // Fecha o DataReader antes de executar qualquer outra operação

                            // Executa o DELETE depois de fechar o DataReader
                         /*   string deleteQuery = "DELETE FROM prTI001_Base_Mensal";
                            SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                            deleteCmd.ExecuteNonQuery(); */

                            // Fecha o leitor de dados antes de prosseguir


                            if (!arquivoExiste) // Se o arquivo não está na tabela
                            {
                                string vArquivoExiste = "0";

                                if (arquivo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                                {
                                    importei = true;

                                    string arquivoBase = Path.Combine(caminho, arquivo.Name);

                                    // Usar ExecuteNonQuery para a inserção em massa
                                    string bulkInsertQuery = $@"
                                BULK INSERT prTI001_Base_Mensal 
                                FROM '{arquivoBase}' 
                                WITH (CODEPAGE = '65001', FIRSTROW = 2, FIELDTERMINATOR = ';', ROWTERMINATOR = '0x0a')";
                                    SqlCommand bulkInsertCmd = new SqlCommand(bulkInsertQuery, conn);
                                    bulkInsertCmd.ExecuteNonQuery();
                                    string dataDisponibilizacaoConvertida = dataDisponibilizacao.ToString("yyyy-MM-dd HH:mm:ss.fff");

                                  


                                    string vSQL02 = $"INSERT INTO [dbo].[prTb001_Arquivo]([no_arquivo],[dh_arquivo],[qt_tamanho],[no_endereco_completo]) VALUES ('{arquivo.Name}+{dataDisponibilizacao} ','{dataDisponibilizacaoConvertida}',{tamanhoArquivo},'{enderecoCompleto}')  ";
                                    SqlCommand cmdSQL02 = new SqlCommand(vSQL02, conn);
                                    cmdSQL02.ExecuteNonQuery();

                                    // Tratar arquivos duplicados 
                    string vSQL03 = $"WITH CTE_DUPLICADAS AS ( SELECT *,ROW_NUMBER() OVER(PARTITION BY [co_ctr],[nu_ope],[nu_cpf_cnpj],[ic_caixa],[nu_unidade],[co_ag_relac],[dt_conce],[dd_vencimento],[vlr_concessao] ORDER BY co_ctr) AS BN FROM[qldDB001].[dbo].[prTI001_Base_Mensal]) DELETE FROM CTE_DUPLICADAS WHERE BN > 1; ";
                                    SqlCommand cmdSQL03 = new SqlCommand(vSQL03, conn);
                                    cmdSQL03.ExecuteNonQuery();


                                    // Tratar arquivos duplicados 
                                    string vSQL04 = $"INSERT INTO [qldDB001].[dbo].[prTb002_Base_Mensal_Tratada] ([co_ctr],[nu_ope],[nu_cpf_cnpj],[ic_caixa]" +
     " ,[tp_pessoa],[co_sis],[nu_unidade],[co_ag_relac],[dt_conce],[dd_vencimento],[vlr_concessao],[nu_posicao],[da_ini],[dt_mov],[da_atual],[nu_tabela],[vr_base_calculo]" +
     " ,[co_rat_prov],[co_rat_hh],[co_mod],[no_cart] ,[co_cart],[co_seg],[no_seg],[co_segger],[co_segger_gp],[co_segad],[rat_h5],[rat_h6],[ic_atacado],[ic_reg] ,[ic_rj],[ic_honrado],[am_honrado])" +
"SELECT* FROM[qldDB001].[dbo].[prTI001_Base_Mensal] where[vlr_concessao] > '0'";
                                    SqlCommand cmdSQL04 = new SqlCommand(vSQL04, conn);
                                    cmdSQL04.ExecuteNonQuery();


                                    // Insere na tabela Base Mensal tratada apenas registro com vlr_concessao > 0
                                    string vSQL05 = $"INSERT INTO [qldDB001].[dbo].[prTb002_Base_Mensal_Tratada] ([co_ctr],[nu_ope],[nu_cpf_cnpj],[ic_caixa]" +
     " ,[tp_pessoa],[co_sis],[nu_unidade],[co_ag_relac],[dt_conce],[dd_vencimento],[vlr_concessao],[nu_posicao],[da_ini],[dt_mov],[da_atual],[nu_tabela],[vr_base_calculo]" +
     " ,[co_rat_prov],[co_rat_hh],[co_mod],[no_cart] ,[co_cart],[co_seg],[no_seg],[co_segger],[co_segger_gp],[co_segad],[rat_h5],[rat_h6],[ic_atacado],[ic_reg] ,[ic_rj],[ic_honrado],[am_honrado])" +
"SELECT* FROM[qldDB001].[dbo].[prTI001_Base_Mensal] where[vlr_concessao] > '0'";
                                    SqlCommand cmdSQL05 = new SqlCommand(vSQL04, conn);
                                    cmdSQL05.ExecuteNonQuery();

                                    // Mover o arquivo para o caminho de backup
                                    string arquivoDestino = Path.Combine(caminhoBackup, arquivo.Name);
                                    File.Copy(arquivo.FullName, arquivoDestino, true);
                                    File.Delete(arquivo.FullName);
                                }
                            }
                            else
                            {
                                grupoMensagemAlerta.Attributes["class"] = "input-group mensagemAlerta";

                                mensagemAlerta.Value = "Arquivo já importado.";

                            }

                        }
                    }

                    conn.Close();
                }

            }
        }
    }
}

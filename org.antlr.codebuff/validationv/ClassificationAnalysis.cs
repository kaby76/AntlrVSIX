using System.Collections.Generic;

namespace org.antlr.codebuff.validation
{


	public class ClassificationAnalysis
	{
		public int n_ws_decisions = 0; // should be number of real tokens - 2 (we don't process 1st token or EOF)
		public int n_actual_none = 0;
		public int n_actual_nl = 0;
		public int n_actual_sp = 0;

		public int n_actual_align = 0;
		public int n_actual_align_child = 0;
		public int n_actual_indent_child = 0;
		public int n_actual_indent = 0;
		public int n_align_decisions = 0;

		public int n_ws_errors;
		public int n_align_errors;

		public int correct_none = 0;
		public int correct_nl = 0;
		public int correct_sp = 0;

		public int correct_align = 0;
		public int correct_align_child = 0;
		public int correct_indent_child = 0;
		public int correct_indent = 0;

		public InputDocument testDoc;
		public IList<TokenPositionAnalysis> analysisPerToken;

		public ClassificationAnalysis(InputDocument testDoc, IList<TokenPositionAnalysis> analysisPerToken)
		{
			this.analysisPerToken = analysisPerToken;
			this.testDoc = testDoc;
			computeAccuracy();
		}

		public virtual void computeAccuracy()
		{
			foreach (TokenPositionAnalysis a in analysisPerToken)
			{
				if (a == null)
				{
					continue;
				}
				n_ws_decisions++;
				// count actual predictions
				bool actual_ws_none = a.actualWS == 0;
				bool actual_nl = (a.actualWS & 0xFF) == Trainer.CAT_INJECT_NL;
				bool actual_ws = (a.actualWS & 0xFF) == Trainer.CAT_INJECT_WS;
				if (actual_ws_none)
				{
					n_actual_none++;
				}
				else if (actual_nl)
				{
					n_actual_nl++;
				}
				else if (actual_ws)
				{
					n_actual_sp++;
				}

				bool predict_ws_none = (a.wsPrediction & 0xFF) == Trainer.CAT_NO_WS;
				bool predict_nl = (a.wsPrediction & 0xFF) == Trainer.CAT_INJECT_NL;
				bool predict_sp = (a.wsPrediction & 0xFF) == Trainer.CAT_INJECT_WS;
				if (predict_ws_none && actual_ws_none)
				{
					correct_none++;
				}
				else if (predict_nl && a.wsPrediction == a.actualWS)
				{
					correct_nl++;
				}
				else if (predict_sp && a.wsPrediction == a.actualWS)
				{
					correct_sp++;
				}
				else
				{
					n_ws_errors++;
				}

				bool actual_align = (a.actualAlign & 0xFF) == Trainer.CAT_ALIGN;
				bool actual_align_child = (a.actualAlign & 0xFF) == Trainer.CAT_ALIGN_WITH_ANCESTOR_CHILD;
				bool actual_indent_child = (a.actualAlign & 0xFF) == Trainer.CAT_INDENT_FROM_ANCESTOR_CHILD;
				bool actual_indent = (a.actualAlign & 0xFF) == Trainer.CAT_INDENT;

				bool predict_align = (a.alignPrediction & 0xFF) == Trainer.CAT_ALIGN;
				bool predict_align_child = (a.alignPrediction & 0xFF) == Trainer.CAT_ALIGN_WITH_ANCESTOR_CHILD;
				bool predict_indent_child = (a.alignPrediction & 0xFF) == Trainer.CAT_INDENT_FROM_ANCESTOR_CHILD;
				bool predict_indent = (a.alignPrediction & 0xFF) == Trainer.CAT_INDENT;

				// if we predicted newline *and* actual was newline, check alignment misclassifications
				if (predict_nl && actual_nl)
				{
					if (actual_align_child)
					{
						n_actual_align_child++;
					}
					else if (actual_indent_child)
					{
						n_actual_indent_child++;
					}
					else if (actual_indent)
					{
						n_actual_indent++;
					}
					else if (actual_align)
					{
						n_actual_align++;
					}
					// Can't compare align/indent if both aren't supposed to align.
					// If we predict '\n' but actual is ' ', alignment will always fail
					// to match. Similarly, if we predict no-'\n' but actual is '\n',
					// we didn't compute align so can't compare.
					n_align_decisions++;
					if (predict_align && actual_align)
					{
						correct_align++;
					}
					else if (predict_align_child && a.alignPrediction == a.actualAlign)
					{
						correct_align_child++;
					}
					else if (predict_indent_child && a.alignPrediction == a.actualAlign)
					{
						correct_indent_child++;
					}
					else if (predict_indent && a.alignPrediction == a.actualAlign)
					{
						correct_indent++;
					}
					else
					{
						n_align_errors++;
					}
				}
			}
		}

		public override string ToString()
		{
			float none_accuracy = correct_none / (float) n_actual_none;
			float nl_accuracy = correct_nl / (float) n_actual_nl;
			float sp_accuracy = correct_sp / (float) n_actual_sp;
			double overall_ws_accuracy = (correct_none + correct_nl + correct_sp) / (float)(n_actual_none + n_actual_nl + n_actual_sp);

			float align_none_accuracy = correct_align / (float) n_actual_align;
			float align_child_accuracy = correct_align_child / (float) n_actual_align_child;
			float indent_child_accuracy = correct_indent_child / (float) n_actual_indent_child;
			float indent_accuracy = correct_indent / (float) n_actual_indent;
			float overall_align_accuracy = (correct_align_child + correct_indent_child + correct_indent) / (float) n_align_decisions;

			string s = "%5d tokens\n" + "%5d ws decisions (should differ from tokens by 2)\n" + "%5d alignment decisions\n" + "\n" + "no ws          %4d/%4d = %5.2f%%\n" + "nl             %4d/%4d = %5.2f%%\n" + "sp             %4d/%4d = %5.2f%%\n" + "overall ws     %4d/%4d = %5.2f%%\n" + "ws errors      %4d/%4d\n" + "\n" + "align          %4d/%4d = %5.2f%%\n" + "align ^ child  %4d/%4d = %5.2f%%\n" + "indent ^ child %4d/%4d = %5.2f%%\n" + "indent         %4d/%4d = %5.2f%%\n" + "overall align              %5.2f%%\n" + "align errors   %4d/%4d\n" + "\n" + "overall error  %4d/%4d = %5.2f%%\n";
			return string.format(s, testDoc.tokens.RealTokens.Count, n_ws_decisions, n_align_decisions, correct_none, n_actual_none, none_accuracy * 100, correct_nl, n_actual_nl, nl_accuracy * 100, correct_sp, n_actual_sp, sp_accuracy * 100, (correct_none + correct_nl + correct_sp), (n_actual_none + n_actual_nl + n_actual_sp), overall_ws_accuracy * 100, n_ws_errors, n_ws_decisions, correct_align, n_actual_align, align_none_accuracy * 100, correct_align_child, n_actual_align_child, align_child_accuracy * 100, correct_indent_child, n_actual_indent_child, indent_child_accuracy * 100, correct_indent, n_actual_indent, indent_accuracy * 100, overall_align_accuracy * 100, n_align_errors, n_align_decisions, n_ws_errors + n_align_errors, n_ws_decisions + n_align_decisions, ErrorRate * 100.0);
		}

		public virtual float ErrorRate
		{
			get
			{
				return ((float)n_ws_errors + n_align_errors) / (n_ws_decisions + n_align_decisions);
			}
		}

		public virtual float WSErrorRate
		{
			get
			{
				return ((float)n_ws_errors) / n_ws_decisions;
			}
		}

		public virtual float AlignmentErrorRate
		{
			get
			{
				return ((float)n_align_errors) / n_align_decisions;
			}
		}
	}

}
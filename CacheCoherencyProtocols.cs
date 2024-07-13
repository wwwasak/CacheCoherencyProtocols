// Sample Assignment Template - Arithmetic Operations. S Manoharan.
//
// There are three parts to an assignment: 
//    (1) assignment specification in HTML
//    (2) answer sheet in plaintext 
//    (3) marking result in plaintext
// These parts are served as separate endpoints, and they are 
// independent of each other in the HTTP sense - they are stateless
// or often called RESTful. Any initialization using student ID
// must be done for each of the three endpoints. Initialization
// in the case of this example is done via the method InitInputs
// and this method must therefore be called in all three of the
// endpoint contexts. Thus, at the beginning of the HTML template 
// you see this call: <p class="cws_code_q">InitInputs</p>
// Similary, both of the Answers() and MarkingResult()
// call InitInputs() before they do anything else.

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities.Courses
{
    public class CacheCoherencyProtocols911659305 : TemplatedCoursework
    {
        public override void Initialize()
        {
            base.Initialize();
        } // Initialize
        public enum CacheLineState
        {
            Modified,
            Owned,
            Shared,
            Invalid,
            Exclusive
        }
        public class CacheLine
        {
            public CacheLineState State { get; set; }

            public CacheLine()
            {
                State = CacheLineState.Invalid;
            }
        }
        public string InitInputs(uint uid)
        {
            Random random = GetRandom(uid);
            string[] cores = { "Core A", "Core B", "Core C", "Core D" };
            string[] operations = { "read", "write" };
            string address = "X";
            CacheCoherencySimulator MSIsimulator = new CacheCoherencySimulator("MSI");
            CacheCoherencySimulator MESIsimulator = new CacheCoherencySimulator("MESI");
            CacheCoherencySimulator MOSIsimulator = new CacheCoherencySimulator("MOSI");
            CacheCoherencySimulator MOESIsimulator = new CacheCoherencySimulator("MOESI");
            listOperations.Clear();
            MSIAnswer.Clear();
            MESIAnswer.Clear();
            MOSIAnswer.Clear();
            MOESIAnswer.Clear();
            for (int i = 0; i < 5; i++)
            {
                string core = cores[random.Next(cores.Length)];
                string operation = operations[random.Next(operations.Length)];
                MSIsimulator.ProcessOperation(core, operation, address);
                MESIsimulator.ProcessOperation(core, operation, address);
                MOSIsimulator.ProcessOperation(core, operation, address);
                MOESIsimulator.ProcessOperation(core, operation, address);
                listOperations.Add($"{core} {operation} {address}");
                MSIAnswer.Add($"{MSIsimulator.GetCacheStates(address)},{MSIsimulator.GetmemoryWriteBack()},{MSIsimulator.GetInvalidationOperations()}");
                MESIAnswer.Add($"{MESIsimulator.GetCacheStates(address)},{MESIsimulator.GetmemoryWriteBack()},{MESIsimulator.GetInvalidationOperations()}");
                MOSIAnswer.Add($"{MOSIsimulator.GetCacheStates(address)},{MOSIsimulator.GetmemoryWriteBack()},{MOSIsimulator.GetInvalidationOperations()}");
                MOESIAnswer.Add($"{MOESIsimulator.GetCacheStates(address)},{MOESIsimulator.GetmemoryWriteBack()},{MOESIsimulator.GetInvalidationOperations()}");
            }
            return "";


        } // InitInputs

        public override string Answers(bool withAnswers, uint uid)
        {
            InitInputs(uid);
            var sb = new StringBuilder();
            sb.AppendFormat($"AUID: {uid}");
            sb.AppendLine();
            for (int i = 1; i <= 5; i++)
            {
                WriteAnswers(sb, GetMSI(uid, i), $"1-{i}", withAnswers);
            }
            for (int i = 1; i <= 5; i++)
            {
                WriteAnswers(sb, GetMESI(uid, i), $"2-{i}", withAnswers);
            }
            for (int i = 1; i <= 5; i++)
            {
                WriteAnswers(sb, GetMOSI(uid, i), $"3-{i}", withAnswers);
            }
            for (int i = 1; i <= 5; i++)
            {
                WriteAnswers(sb, GetMOESI(uid, i), $"4-{i}", withAnswers);
            }
            return sb.ToString();
        } // Answers

        private void WriteAnswers(StringBuilder sb, string answer, string partId, bool withAnswers)
        {
            sb.AppendFormat($"{partId}: {(withAnswers ? answer : string.Empty)}");
            sb.AppendLine();
        } // WriteAnswers

        public override string MarkingResult(bool withAnswers, uint uid, string submission, out double mark)
        {
            InitInputs(uid);
            mark = 0.0;
            var sb = new StringBuilder();

            Dictionary<string, string> kvp = ProcessAnswerFile(submission);
            string[] answers1 = new string[5];
            string[] answers2 = new string[5];
            string[] answers3 = new string[5];
            string[] answers4 = new string[5];
            for (int i = 0; i < 5; i++)
            {
                answers1[i] = SubmittedAnswer(kvp, $"1-{i + 1}").Replace(" ", "");
            }
            for (int i = 0; i < 5; i++)
            {
                answers2[i] = SubmittedAnswer(kvp, $"2-{i + 1}").Replace(" ", "");
            }
            for (int i = 0; i < 5; i++)
            {
                answers3[i] = SubmittedAnswer(kvp, $"3-{i + 1}").Replace(" ", "");
            }
            for (int i = 0; i < 5; i++)
            {
                answers4[i] = SubmittedAnswer(kvp, $"4-{i + 1}").Replace(" ", "");
            }

            if (withAnswers)
            {
                for (int i = 1; i <= 5; i++)
                {
                    sb.AppendLine($"1-{i}: correct answer: [{GetMSI(uid, i)}]; your answer: [{answers1[i - 1]}]");
                }
                for (int i = 1; i <= 5; i++)
                {
                    sb.AppendLine($"2-{i}: correct answer: [{GetMESI(uid, i)}]; your answer: [{answers2[i - 1]}]");
                }
                for (int i = 1; i <= 5; i++)
                {
                    sb.AppendLine($"3-{i}: correct answer: [{GetMOSI(uid, i)}]; your answer: [{answers3[i - 1]}]");
                }
                for (int i = 1; i <= 5; i++)
                {
                    sb.AppendLine($"4-{i}: correct answer: [{GetMOESI(uid, i)}]; your answer: [{answers4[i - 1]}]");
                }
            }
            for (int i = 0; i < answers1.Length; i++)
            {
                if (string.Equals(answers1[i], GetMSI(uid, i + 1), StringComparison.OrdinalIgnoreCase))
                {
                    mark += 1;
                }
                else
                {
                    sb.AppendLine($"1-{i + 1}: INCORRECT ANSWER");
                }
            }

            for (int i = 0; i < answers2.Length; i++)
            {
                if (string.Equals(answers2[i], GetMESI(uid, i + 1), StringComparison.OrdinalIgnoreCase))
                {
                    mark += 1;
                }
                else
                {
                    sb.AppendLine($"2-{i + 1}: INCORRECT ANSWER");
                }
            }

            for (int i = 0; i < answers3.Length; i++)
            {
                if (string.Equals(answers3[i], GetMOSI(uid, i + 1), StringComparison.OrdinalIgnoreCase))
                {
                    mark += 1;
                }
                else
                {
                    sb.AppendLine($"3-{i + 1}: INCORRECT ANSWER");
                }
            }

            for (int i = 0; i < answers4.Length; i++)
            {
                if (string.Equals(answers4[i], GetMOESI(uid, i + 1), StringComparison.OrdinalIgnoreCase))
                {
                    mark += 1;
                }
                else
                {
                    sb.AppendLine($"4-{i + 1}: INCORRECT ANSWER");
                }
            }


            sb.AppendLine();
            sb.AppendLine($"Your total marks: {mark}/20");
            return sb.ToString();
        } // MarkingResult

        private static string SubmittedAnswer(Dictionary<string, string> kvp, string qId)
        {
            string a = kvp.ContainsKey(qId) ? Regex.Replace(kvp[qId], @"\s+", "") : "";
            return a;
        } // SubmittedAnswer

        public string GetOperations(uint _)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < listOperations.Count; ++i)
            {
                sb.Append($"<p>{listOperations[i]}</p>");
            }
            return sb.ToString();
        } // GetOperation

        public string GetMSI(uint _, int order)
        {
            var sb = new StringBuilder();
            sb.Append($"{MSIAnswer[order - 1]}");
            return sb.ToString();
        } // GetMSI
        public string GetMSIAll(uint _)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 5; ++i)
            {
                sb.Append($"{MSIAnswer[i]}<br/>");
            }
            return sb.ToString();
        }

        public string GetMESI(uint _, int order)
        {
            var sb = new StringBuilder();
            sb.Append($"{MESIAnswer[order - 1]}");
            return sb.ToString();
        } // GetMESI
        public string GetMESIAll(uint _)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 5; ++i)
            {
                sb.Append($"{MESIAnswer[i]}<br/>");
            }
            return sb.ToString();
        }
        public string GetMOSI(uint _, int order)
        {
            var sb = new StringBuilder();
            sb.Append($"{MOSIAnswer[order - 1]}");
            return sb.ToString();
        } // GetMOSI
        public string GetMOSIAll(uint _)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 5; ++i)
            {
                sb.Append($"{MOSIAnswer[i]}<br/>");
            }
            return sb.ToString();
        }
        public string GetMOESI(uint _, int order)
        {
            var sb = new StringBuilder();
            sb.Append($"{MOESIAnswer[order - 1]}");
            return sb.ToString();
        } // GetMOESI
        public string GetMOESIAll(uint _)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 5; ++i)
            {
                sb.Append($"{MOESIAnswer[i]}<br/>");
            }
            return sb.ToString();
        }

        private List<string> listOperations = new();
        private List<string> MSIAnswer = new();
        private List<string> MESIAnswer = new();
        private List<string> MOSIAnswer = new();
        private List<string> MOESIAnswer = new();
        public class CacheCoherencySimulator
        {
            private Dictionary<string, CacheLine[]> cache;
            private int memoryWriteBack;
            private int invalidationOperations;
            private string protocol;

            public CacheCoherencySimulator(string protocol)
            {
                cache = new Dictionary<string, CacheLine[]>();
                memoryWriteBack = 0;
                invalidationOperations = 0;
                this.protocol = protocol;
            }

            public void ProcessOperation(string core, string operation, string address)
            {
                if (!cache.ContainsKey(address))
                {
                    cache[address] = new CacheLine[4] { new CacheLine(), new CacheLine(), new CacheLine(), new CacheLine() };
                }

                int coreIndex = GetCoreIndex(core);
                CacheLine[] cacheLines = cache[address];

                if (operation == "read")
                {
                    ProcessRead(coreIndex, cacheLines);
                }
                else if (operation == "write")
                {
                    ProcessWrite(coreIndex, cacheLines);
                }
            }

            private void ProcessRead(int coreIndex, CacheLine[] cacheLines)
            {
                CacheLine currentLine = cacheLines[coreIndex];

                switch (protocol)
                {
                    case "MSI":
                        ProcessReadMSI(coreIndex, cacheLines, currentLine);
                        break;
                    case "MOSI":
                        ProcessReadMOSI(coreIndex, cacheLines, currentLine);
                        break;
                    case "MESI":
                        ProcessReadMESI(coreIndex, cacheLines, currentLine);
                        break;
                    case "MOESI":
                        ProcessReadMOESI(coreIndex, cacheLines, currentLine);
                        break;
                }
            }

            private void ProcessWrite(int coreIndex, CacheLine[] cacheLines)
            {
                CacheLine currentLine = cacheLines[coreIndex];

                switch (protocol)
                {
                    case "MSI":
                        ProcessWriteMSI(coreIndex, cacheLines, currentLine);
                        break;
                    case "MOSI":
                        ProcessWriteMOSI(coreIndex, cacheLines, currentLine);
                        break;
                    case "MESI":
                        ProcessWriteMESI(coreIndex, cacheLines, currentLine);
                        break;
                    case "MOESI":
                        ProcessWriteMOESI(coreIndex, cacheLines, currentLine);
                        break;
                }
            }

            private void ProcessReadMSI(int coreIndex, CacheLine[] cacheLines, CacheLine currentLine)
            {
                switch (currentLine.State)
                {
                    case CacheLineState.Invalid:
                        foreach (var line in cacheLines)
                        {
                            if (line.State == CacheLineState.Modified)
                            {
                                memoryWriteBack++;
                                line.State = CacheLineState.Shared;
                                currentLine.State = CacheLineState.Shared;
                                return;
                            }
                            if (line.State == CacheLineState.Shared)
                            {
                                currentLine.State = CacheLineState.Shared;
                                return;
                            }
                        }
                        currentLine.State = CacheLineState.Shared;
                        break;
                    case CacheLineState.Shared:
                    case CacheLineState.Modified:
                        break;
                }
            }

            private void ProcessWriteMSI(int coreIndex, CacheLine[] cacheLines, CacheLine currentLine)
            {
                switch (currentLine.State)
                {
                    case CacheLineState.Invalid:
                        foreach (var line in cacheLines)
                        {
                            if (line.State == CacheLineState.Modified)
                            {
                                memoryWriteBack++;
                                InvalidateOtherCaches(coreIndex, cacheLines);
                                currentLine.State = CacheLineState.Modified;
                                return;
                            }
                            if (line.State == CacheLineState.Shared)
                            {
                                InvalidateOtherCaches(coreIndex, cacheLines);
                                currentLine.State = CacheLineState.Modified;
                                return;
                            }
                        }
                        InvalidateOtherCaches(coreIndex, cacheLines);
                        currentLine.State = CacheLineState.Modified;
                        break;
                    case CacheLineState.Shared:
                        InvalidateOtherCaches(coreIndex, cacheLines);
                        currentLine.State = CacheLineState.Modified;
                        break;
                    case CacheLineState.Modified:
                        break;
                }
            }

            private void ProcessReadMOSI(int coreIndex, CacheLine[] cacheLines, CacheLine currentLine)
            {
                switch (currentLine.State)
                {
                    case CacheLineState.Invalid:
                        foreach (var line in cacheLines)
                        {
                            if (line.State == CacheLineState.Modified)
                            {
                                line.State = CacheLineState.Owned;
                                currentLine.State = CacheLineState.Shared;
                                return;
                            }
                            if (line.State == CacheLineState.Owned)
                            {
                                currentLine.State = CacheLineState.Shared;
                                return;
                            }
                        }
                        currentLine.State = CacheLineState.Shared;
                        break;
                    case CacheLineState.Shared:
                    case CacheLineState.Owned:
                    case CacheLineState.Modified:
                        break;
                }
            }

            private void ProcessWriteMOSI(int coreIndex, CacheLine[] cacheLines, CacheLine currentLine)
            {
                switch (currentLine.State)
                {
                    case CacheLineState.Invalid:
                        foreach (var line in cacheLines)
                        {
                            if (line.State == CacheLineState.Modified)
                            {
                                memoryWriteBack++;
                                InvalidateOtherCaches(coreIndex, cacheLines);
                                currentLine.State = CacheLineState.Modified;
                                return;
                            }
                            if (line.State == CacheLineState.Shared)
                            {
                                InvalidateOtherCaches(coreIndex, cacheLines);
                                currentLine.State = CacheLineState.Modified;
                                return;
                            }
                            if (line.State == CacheLineState.Owned)
                            {
                                InvalidateOtherCaches(coreIndex, cacheLines);
                                currentLine.State = CacheLineState.Modified;
                                return;
                            }
                        }
                        InvalidateOtherCaches(coreIndex, cacheLines);
                        currentLine.State = CacheLineState.Modified;
                        break;
                    case CacheLineState.Owned:
                    case CacheLineState.Shared:
                        InvalidateOtherCaches(coreIndex, cacheLines);
                        currentLine.State = CacheLineState.Modified;
                        break;
                    case CacheLineState.Modified:
                        break;
                }
            }

            private void ProcessReadMESI(int coreIndex, CacheLine[] cacheLines, CacheLine currentLine)
            {
                switch (currentLine.State)
                {
                    case CacheLineState.Invalid:
                        foreach (var line in cacheLines)
                        {
                            if (line.State == CacheLineState.Modified)
                            {
                                memoryWriteBack++;
                                line.State = CacheLineState.Shared;
                                currentLine.State = CacheLineState.Shared;
                                return;
                            }
                            if (line.State == CacheLineState.Shared)
                            {
                                currentLine.State = CacheLineState.Shared;
                                return;
                            }
                            if (line.State == CacheLineState.Exclusive)
                            {
                                memoryWriteBack++;
                                line.State = CacheLineState.Shared;
                                currentLine.State = CacheLineState.Shared;
                                return;
                            }
                        }
                        currentLine.State = CacheLineState.Exclusive;
                        break;
                    case CacheLineState.Shared:
                    case CacheLineState.Modified:
                    case CacheLineState.Exclusive:
                        break;
                }
            }

            private void ProcessWriteMESI(int coreIndex, CacheLine[] cacheLines, CacheLine currentLine)
            {
                switch (currentLine.State)
                {
                    case CacheLineState.Invalid:
                        foreach (var line in cacheLines)
                        {
                            if (line.State == CacheLineState.Modified)
                            {
                                memoryWriteBack++;
                                InvalidateOtherCaches(coreIndex, cacheLines);
                                currentLine.State = CacheLineState.Modified;
                                return;
                            }
                            if (line.State == CacheLineState.Exclusive)
                            {
                                memoryWriteBack++;
                                InvalidateOtherCaches(coreIndex, cacheLines);
                                currentLine.State = CacheLineState.Modified;
                                return;
                            }
                            if (line.State == CacheLineState.Shared)
                            {
                                InvalidateOtherCaches(coreIndex, cacheLines);
                                currentLine.State = CacheLineState.Modified;
                                return;
                            }
                        }
                        InvalidateOtherCaches(coreIndex, cacheLines);
                        currentLine.State = CacheLineState.Exclusive;
                        break;
                    case CacheLineState.Shared:
                        InvalidateOtherCaches(coreIndex, cacheLines);
                        currentLine.State = CacheLineState.Modified;
                        break;
                    case CacheLineState.Exclusive:
                        currentLine.State = CacheLineState.Modified;
                        break;
                    case CacheLineState.Modified:
                        break;
                }
            }

            private void ProcessReadMOESI(int coreIndex, CacheLine[] cacheLines, CacheLine currentLine)
            {
                switch (currentLine.State)
                {
                    case CacheLineState.Invalid:
                        foreach (var line in cacheLines)
                        {
                            if (line.State == CacheLineState.Modified)
                            {
                                line.State = CacheLineState.Owned;
                                currentLine.State = CacheLineState.Shared;
                                return;
                            }
                            if (line.State == CacheLineState.Owned)
                            {
                                currentLine.State = CacheLineState.Shared;
                                return;
                            }
                            if (line.State == CacheLineState.Shared)
                            {
                                currentLine.State = CacheLineState.Shared;
                                return;
                            }
                            if (line.State == CacheLineState.Exclusive)
                            {
                                line.State = CacheLineState.Shared;
                                currentLine.State = CacheLineState.Shared;
                                return;
                            }
                        }
                        currentLine.State = CacheLineState.Exclusive;
                        break;
                    case CacheLineState.Shared:
                    case CacheLineState.Owned:
                    case CacheLineState.Modified:
                    case CacheLineState.Exclusive:
                        break;
                }
            }

            private void ProcessWriteMOESI(int coreIndex, CacheLine[] cacheLines, CacheLine currentLine)
            {
                switch (currentLine.State)
                {
                    case CacheLineState.Invalid:
                        foreach (var line in cacheLines)
                        {
                            if (line.State == CacheLineState.Exclusive)
                            {
                                memoryWriteBack++;
                                InvalidateOtherCaches(coreIndex, cacheLines);
                                currentLine.State = CacheLineState.Modified;
                                return;
                            }
                            if (line.State == CacheLineState.Modified)
                            {
                                memoryWriteBack++;
                                InvalidateOtherCaches(coreIndex, cacheLines);
                                currentLine.State = CacheLineState.Modified;
                                return;
                            }
                            if (line.State == CacheLineState.Shared)
                            {
                                InvalidateOtherCaches(coreIndex, cacheLines);
                                currentLine.State = CacheLineState.Modified;
                                return;
                            }
                            if (line.State == CacheLineState.Owned)
                            {
                                InvalidateOtherCaches(coreIndex, cacheLines);
                                currentLine.State = CacheLineState.Modified;
                                return;
                            }
                        }
                        InvalidateOtherCaches(coreIndex, cacheLines);
                        currentLine.State = CacheLineState.Exclusive;
                        break;
                    case CacheLineState.Owned:
                    case CacheLineState.Shared:
                        InvalidateOtherCaches(coreIndex, cacheLines);
                        currentLine.State = CacheLineState.Modified;
                        break;
                    case CacheLineState.Exclusive:
                        currentLine.State = CacheLineState.Modified;
                        break;
                    case CacheLineState.Modified:
                        break;
                }
            }

            private void InvalidateOtherCaches(int coreIndex, CacheLine[] cacheLines)
            {
                for (int i = 0; i < cacheLines.Length; i++)
                {
                    if (i != coreIndex)
                    {
                        cacheLines[i].State = CacheLineState.Invalid;
                    }
                }
                invalidationOperations++;
            }

            private int GetCoreIndex(string core)
            {
                switch (core)
                {
                    case "Core A":
                        return 0;
                    case "Core B":
                        return 1;
                    case "Core C":
                        return 2;
                    case "Core D":
                        return 3;
                    default:
                        throw new ArgumentException("Invalid core identifier");
                }
            }

            public string GetCacheStates(string address)
            {
                if (!cache.ContainsKey(address))
                {
                    throw new ArgumentException("Address not found in cache");
                }

                CacheLine[] cacheLines = cache[address];
                string[] stateStrings = new string[cacheLines.Length];
                for (int i = 0; i < cacheLines.Length; i++)
                {
                    stateStrings[i] = cacheLines[i].State.ToString().Substring(0, 1);
                }

                return string.Join(",", stateStrings);
            }

            public int GetmemoryWriteBack()
            {
                return memoryWriteBack;
            }

            public int GetInvalidationOperations()
            {
                return invalidationOperations;
            }
        }
    } // class
} // namespace

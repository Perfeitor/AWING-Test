import {useState} from "react";
import {
    Box,
    Button,
    Container,
    Grid,
    TextField,
    Typography,
    Paper,
} from "@mui/material";
import axios from "axios";

export default function App() {
    const [n, setN] = useState(3);
    const [m, setM] = useState(3);
    const [p, setP] = useState(1);

    const [matrix, setMatrix] = useState<number[][]>([
        [1, 1, 1],
        [1, 1, 1],
        [1, 1, 1],
    ]);

    const [result, setResult] = useState<number | null>(null);

    const updateMatrixSize = (nVal: number, mVal: number) => {
        const newMatrix: number[][] = [];
        for (let i = 0; i < nVal; i++) {
            newMatrix[i] = [];
            for (let j = 0; j < mVal; j++) {
                newMatrix[i][j] = matrix[i]?.[j] ?? 1;
            }
        }
        setMatrix(newMatrix);
    };

    const calculateP = (mat: number[][]) => {
        const allValues = mat.flat();
        return Math.max(...allValues);
    };
    
    const handleMatrixChange = (i: number, j: number, val: string) => {
        const copied = matrix.map((row) => [...row]);
        const numberVal = Number(val);
        
        if (numberVal <= 0 || isNaN(numberVal)) return;

        copied[i][j] = numberVal;
        setMatrix(copied);
        setP(calculateP(copied));
    };

    const validateInput = () => {
        if (n <= 0 || m <= 0) return "n and m must be positive numbers";

        // flatten matrix
        const flat = matrix.flat();

        // check all positive
        if (flat.some(x => x <= 0)) return "All matrix values must be positive integers";

        // check p appears exactly once
        const countP = flat.filter(x => x === p).length;
        if (countP !== 1) return "The maximum value (p) must appear exactly once in the matrix";

        return null;
    };
    
    const callApi = async () => {
        try {
            const err = validateInput();
            if (err) {
                alert(err);
                return;
            }
            const body = {n, m, p, matrix};
            const res = await axios.post("http://localhost:5217/Awing/calculate-fuel", body);
            setResult(res.data);
        } catch (err: any) {
            alert(err?.response?.data?.message ?? err.message);
        }
    };

    return (
        <Box sx={{background: "#1e1e1e", width: "100vw", py: 2}}>
            <Container maxWidth="md" sx={{ padding: "16px" }}>
                <Paper
                    elevation={8}
                    sx={{
                        p: 4,
                        borderRadius: 3,
                        backgroundColor: "#fff",
                    }}
                >
                    <Typography
                        variant="h4"
                        sx={{fontWeight: 600, mb: 4, textAlign: "center"}}
                    >
                        Treasure Path Calculator
                    </Typography>

                    {/* Top input row */}
                    <Grid container spacing={3}>
                        <Grid item xs={12} sm={4}>
                            <TextField
                                fullWidth
                                label="n (rows)"
                                type="number"
                                value={n}
                                onChange={(e) => {
                                    const v = Number(e.target.value);
                                    setN(v);
                                    updateMatrixSize(v, m);
                                }}
                            />
                        </Grid>

                        <Grid item xs={12} sm={4}>
                            <TextField
                                fullWidth
                                label="m (columns)"
                                type="number"
                                value={m}
                                onChange={(e) => {
                                    const v = Number(e.target.value);
                                    setM(v);
                                    updateMatrixSize(n, v);
                                }}
                            />
                        </Grid>

                        <Grid item xs={12} sm={4}>
                            <TextField
                                fullWidth
                                label="p"
                                value={p}
                                disabled
                            />
                        </Grid>
                    </Grid>

                    <Typography
                        variant="h6"
                        sx={{mt: 4, mb: 2, fontWeight: 600}}
                    >
                        Matrix Input
                    </Typography>

                    <Grid container direction="column" spacing={1}>
                        {matrix.map((row, i) => (
                            <Grid item key={i}>
                                <Grid container spacing={1} justifyContent="flex-start">
                                    {row.map((cell, j) => (
                                        <Grid item key={`${i}-${j}`}>
                                            <TextField
                                                type="number"
                                                value={cell}
                                                onChange={(e) =>
                                                    handleMatrixChange(i, j, e.target.value)
                                                }
                                                sx={{
                                                    width: 70,
                                                    backgroundColor: "#f9f9f9",
                                                    borderRadius: 1,
                                                }}
                                            />
                                        </Grid>
                                    ))}
                                </Grid>
                            </Grid>
                        ))}
                    </Grid>

                    <Box sx={{mt: 4, textAlign: "center"}}>
                        <Button
                            variant="contained"
                            size="large"
                            sx={{
                                px: 6,
                                py: 1.5,
                                fontWeight: 600,
                                fontSize: "1rem",
                                borderRadius: 2,
                            }}
                            onClick={callApi}
                        >
                            CALCULATE FUEL
                        </Button>
                    </Box>

                    {result !== null && (
                        <Typography
                            variant="h6"
                            sx={{mt: 3, textAlign: "center", fontWeight: 500}}
                        >
                            Result: {result}
                        </Typography>
                    )}
                </Paper>
            </Container>
        </Box>
    );
}

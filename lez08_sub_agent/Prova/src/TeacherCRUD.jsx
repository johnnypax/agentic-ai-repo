import { Container, Row, Col, Table, Badge, Alert } from 'react-bootstrap'

const INITIAL_TEACHERS = [
  { id: 1, nome: 'Anna', cognome: 'Ferrari', email: 'anna.ferrari@scuola.it', materia: 'Matematica' },
  { id: 2, nome: 'Carlo', cognome: 'Esposito', email: 'carlo.esposito@scuola.it', materia: 'Storia' },
  { id: 3, nome: 'Elena', cognome: 'Conti', email: 'elena.conti@scuola.it', materia: 'Informatica' },
]

function materiaBadgeVariant(materia) {
  const map = {
    Matematica: 'primary',
    Storia: 'secondary',
    Informatica: 'success',
  }
  return map[materia] ?? 'info'
}

function TeacherCRUD({ teachers = INITIAL_TEACHERS }) {
  return (
    <Container className="py-4">
      <Row className="mb-3 align-items-center">
        <Col>
          <h1 className="h3 mb-0">Elenco Docenti</h1>
          <small className="text-muted">
            {teachers.length} {teachers.length === 1 ? 'docente' : 'docenti'} registrati
          </small>
        </Col>
      </Row>

      {teachers.length === 0 ? (
        <Alert variant="info">Nessun docente presente.</Alert>
      ) : (
        <Table striped bordered hover responsive>
          <thead className="table-dark">
            <tr>
              <th>#</th>
              <th>Nome</th>
              <th>Cognome</th>
              <th>Email</th>
              <th>Materia</th>
            </tr>
          </thead>
          <tbody>
            {teachers.map((t, index) => (
              <tr key={t.id}>
                <td>{index + 1}</td>
                <td>{t.nome}</td>
                <td>{t.cognome}</td>
                <td>{t.email}</td>
                <td>
                  <Badge bg={materiaBadgeVariant(t.materia)}>{t.materia}</Badge>
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      )}
    </Container>
  )
}

export default TeacherCRUD

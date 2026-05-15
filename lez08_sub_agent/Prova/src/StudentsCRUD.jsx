import { useState } from 'react'
import {
  Container,
  Row,
  Col,
  Table,
  Button,
  Modal,
  Form,
  Alert,
  Badge,
} from 'react-bootstrap'

// Dati iniziali di esempio
const INITIAL_STUDENTS = [
  { id: 1, nome: 'Mario', cognome: 'Rossi', email: 'mario.rossi@example.com', voto: 28 },
  { id: 2, nome: 'Giulia', cognome: 'Bianchi', email: 'giulia.bianchi@example.com', voto: 30 },
  { id: 3, nome: 'Luca', cognome: 'Verdi', email: 'luca.verdi@example.com', voto: 25 },
]

const EMPTY_FORM = { nome: '', cognome: '', email: '', voto: '' }

function badgeVariant(voto) {
  const v = Number(voto)
  if (v >= 28) return 'success'
  if (v >= 24) return 'warning'
  return 'danger'
}

function StudentsCRUD() {
  const [studenti, setStudenti] = useState(INITIAL_STUDENTS)
  const [nextId, setNextId] = useState(4)

  // Stato modale
  const [showModal, setShowModal] = useState(false)
  const [modalMode, setModalMode] = useState('add') // 'add' | 'edit'
  const [editingId, setEditingId] = useState(null)
  const [formData, setFormData] = useState(EMPTY_FORM)
  const [formErrors, setFormErrors] = useState({})

  // Stato conferma eliminazione
  const [showDeleteModal, setShowDeleteModal] = useState(false)
  const [deletingStudent, setDeletingStudent] = useState(null)

  // Messaggio di feedback
  const [feedback, setFeedback] = useState(null)

  // ---- Helpers ----

  function showFeedback(message, variant = 'success') {
    setFeedback({ message, variant })
    setTimeout(() => setFeedback(null), 3000)
  }

  function validate(data) {
    const errors = {}
    if (!data.nome.trim()) errors.nome = 'Il nome è obbligatorio.'
    if (!data.cognome.trim()) errors.cognome = 'Il cognome è obbligatorio.'
    if (!data.email.trim()) {
      errors.email = "L'email è obbligatoria."
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(data.email)) {
      errors.email = "Formato email non valido."
    }
    const voto = Number(data.voto)
    if (data.voto === '' || isNaN(voto)) {
      errors.voto = 'Il voto è obbligatorio.'
    } else if (voto < 18 || voto > 30) {
      errors.voto = 'Il voto deve essere compreso tra 18 e 30.'
    }
    return errors
  }

  function handleFormChange(e) {
    const { name, value } = e.target
    setFormData((prev) => ({ ...prev, [name]: value }))
    // Rimuove l'errore sul campo modificato
    if (formErrors[name]) {
      setFormErrors((prev) => ({ ...prev, [name]: undefined }))
    }
  }

  // ---- Apertura modali ----

  function openAddModal() {
    setModalMode('add')
    setEditingId(null)
    setFormData(EMPTY_FORM)
    setFormErrors({})
    setShowModal(true)
  }

  function openEditModal(studente) {
    setModalMode('edit')
    setEditingId(studente.id)
    setFormData({
      nome: studente.nome,
      cognome: studente.cognome,
      email: studente.email,
      voto: String(studente.voto),
    })
    setFormErrors({})
    setShowModal(true)
  }

  function closeModal() {
    setShowModal(false)
  }

  // ---- CRUD operations ----

  function handleSave() {
    const errors = validate(formData)
    if (Object.keys(errors).length > 0) {
      setFormErrors(errors)
      return
    }

    const payload = {
      nome: formData.nome.trim(),
      cognome: formData.cognome.trim(),
      email: formData.email.trim(),
      voto: Number(formData.voto),
    }

    if (modalMode === 'add') {
      setStudenti((prev) => [...prev, { id: nextId, ...payload }])
      setNextId((n) => n + 1)
      showFeedback(`Studente ${payload.nome} ${payload.cognome} aggiunto con successo.`)
    } else {
      setStudenti((prev) =>
        prev.map((s) => (s.id === editingId ? { ...s, ...payload } : s))
      )
      showFeedback(`Studente ${payload.nome} ${payload.cognome} modificato con successo.`)
    }

    closeModal()
  }

  function openDeleteModal(studente) {
    setDeletingStudent(studente)
    setShowDeleteModal(true)
  }

  function confirmDelete() {
    if (!deletingStudent) return
    setStudenti((prev) => prev.filter((s) => s.id !== deletingStudent.id))
    showFeedback(
      `Studente ${deletingStudent.nome} ${deletingStudent.cognome} eliminato.`,
      'danger'
    )
    setShowDeleteModal(false)
    setDeletingStudent(null)
  }

  function cancelDelete() {
    setShowDeleteModal(false)
    setDeletingStudent(null)
  }

  // ---- Render ----

  return (
    <Container className="py-4">
      <Row className="mb-3 align-items-center">
        <Col>
          <h1 className="h3 mb-0">Gestione Studenti</h1>
          <small className="text-muted">
            {studenti.length} {studenti.length === 1 ? 'studente' : 'studenti'} registrati
          </small>
        </Col>
        <Col xs="auto">
          <Button variant="primary" onClick={openAddModal}>
            + Aggiungi Studente
          </Button>
        </Col>
      </Row>

      {feedback && (
        <Alert variant={feedback.variant} dismissible onClose={() => setFeedback(null)}>
          {feedback.message}
        </Alert>
      )}

      {studenti.length === 0 ? (
        <Alert variant="info">
          Nessuno studente presente. Clicca su <strong>Aggiungi Studente</strong> per iniziare.
        </Alert>
      ) : (
        <Table striped bordered hover responsive>
          <thead className="table-dark">
            <tr>
              <th>#</th>
              <th>Nome</th>
              <th>Cognome</th>
              <th>Email</th>
              <th>Voto</th>
              <th className="text-center">Azioni</th>
            </tr>
          </thead>
          <tbody>
            {studenti.map((s, index) => (
              <tr key={s.id}>
                <td>{index + 1}</td>
                <td>{s.nome}</td>
                <td>{s.cognome}</td>
                <td>{s.email}</td>
                <td>
                  <Badge bg={badgeVariant(s.voto)} className="fs-6">
                    {s.voto}
                  </Badge>
                </td>
                <td className="text-center">
                  <Button
                    variant="outline-primary"
                    size="sm"
                    className="me-2"
                    onClick={() => openEditModal(s)}
                  >
                    Modifica
                  </Button>
                  <Button
                    variant="outline-danger"
                    size="sm"
                    onClick={() => openDeleteModal(s)}
                  >
                    Elimina
                  </Button>
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      )}

      {/* Modale Aggiungi / Modifica */}
      <Modal show={showModal} onHide={closeModal} backdrop="static" centered>
        <Modal.Header closeButton>
          <Modal.Title>
            {modalMode === 'add' ? 'Aggiungi Studente' : 'Modifica Studente'}
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form noValidate>
            <Row className="mb-3">
              <Col>
                <Form.Group controlId="formNome">
                  <Form.Label>Nome</Form.Label>
                  <Form.Control
                    type="text"
                    name="nome"
                    value={formData.nome}
                    onChange={handleFormChange}
                    isInvalid={!!formErrors.nome}
                    placeholder="Es. Mario"
                  />
                  <Form.Control.Feedback type="invalid">
                    {formErrors.nome}
                  </Form.Control.Feedback>
                </Form.Group>
              </Col>
              <Col>
                <Form.Group controlId="formCognome">
                  <Form.Label>Cognome</Form.Label>
                  <Form.Control
                    type="text"
                    name="cognome"
                    value={formData.cognome}
                    onChange={handleFormChange}
                    isInvalid={!!formErrors.cognome}
                    placeholder="Es. Rossi"
                  />
                  <Form.Control.Feedback type="invalid">
                    {formErrors.cognome}
                  </Form.Control.Feedback>
                </Form.Group>
              </Col>
            </Row>

            <Form.Group className="mb-3" controlId="formEmail">
              <Form.Label>Email</Form.Label>
              <Form.Control
                type="email"
                name="email"
                value={formData.email}
                onChange={handleFormChange}
                isInvalid={!!formErrors.email}
                placeholder="Es. mario.rossi@example.com"
              />
              <Form.Control.Feedback type="invalid">
                {formErrors.email}
              </Form.Control.Feedback>
            </Form.Group>

            <Form.Group className="mb-3" controlId="formVoto">
              <Form.Label>Voto (18 - 30)</Form.Label>
              <Form.Control
                type="number"
                name="voto"
                value={formData.voto}
                onChange={handleFormChange}
                isInvalid={!!formErrors.voto}
                min={18}
                max={30}
                placeholder="Es. 28"
              />
              <Form.Control.Feedback type="invalid">
                {formErrors.voto}
              </Form.Control.Feedback>
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={closeModal}>
            Annulla
          </Button>
          <Button variant="primary" onClick={handleSave}>
            {modalMode === 'add' ? 'Aggiungi' : 'Salva Modifiche'}
          </Button>
        </Modal.Footer>
      </Modal>

      {/* Modale Conferma Eliminazione */}
      <Modal show={showDeleteModal} onHide={cancelDelete} centered>
        <Modal.Header closeButton>
          <Modal.Title>Conferma Eliminazione</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          {deletingStudent && (
            <p>
              Sei sicuro di voler eliminare lo studente{' '}
              <strong>
                {deletingStudent.nome} {deletingStudent.cognome}
              </strong>
              ? Questa operazione non può essere annullata.
            </p>
          )}
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={cancelDelete}>
            Annulla
          </Button>
          <Button variant="danger" onClick={confirmDelete}>
            Elimina
          </Button>
        </Modal.Footer>
      </Modal>
    </Container>
  )
}

export default StudentsCRUD

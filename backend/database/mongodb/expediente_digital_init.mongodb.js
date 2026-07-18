use multibanca;

db.createCollection("expediente_digital");
db.createCollection("sequences");

db.sequences.updateOne(
  { _id: "expediente_digital" },
  { $setOnInsert: { sequence_value: 0 } },
  { upsert: true }
);

db.expediente_digital.createIndex(
  { id_expediente: 1, row_status: 1, is_active: 1 },
  { name: "idx_exp_digital_expediente_estado" }
);

db.expediente_digital.createIndex(
  { id_archivo: 1 },
  { name: "ux_exp_digital_id_archivo", unique: true }
);

db.expediente_digital.createIndex(
  { id_expediente: 1, id_documento: 1, version_archivo: -1 },
  { name: "idx_exp_digital_version_documento" }
);

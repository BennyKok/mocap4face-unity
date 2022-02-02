// Facemoji Mocap4Face API for Unity
// Fork of @BennyKok's Android wrapper.
// iOS port by Thomas Suarez (@tomthecarrot) from Teleportal (@0xTELEPORTAL)

void onActivate(bool isActive);

void addBlendShapeName(const char* newBlendShapeName);
void commitBlendShapeNames();
void setBlendShapeCount(uint16_t count);

void setBlendShapeValue(uint16_t idx, float newValue);
void resetBlendShapeValues();

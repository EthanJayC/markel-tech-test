export interface Company {
  id: number;
  name: string;
  address1: string | null;
  address2: string | null;
  address3: string | null;
  postcode: string | null;
  country: string | null;
  active: boolean;
  insuranceEndDate: string;
  hasActiveInsurancePolicy: boolean;
}

export interface Claim {
  ucr: string;
  companyId: number;
  claimDate: string;
  lossDate: string;
  assuredName: string;
  incurredLoss: number;
  closed: boolean;
  ageInDays: number;
}

export interface UpdateClaimRequest {
  claimDate: string;
  lossDate: string;
  assuredName: string;
  incurredLoss: number;
  closed: boolean;
}

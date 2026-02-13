import { describe, it, expect, vi, beforeEach } from "vitest";
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { BrowserRouter } from "react-router-dom";
import Scan from "../pages/Scan";

// Mock fetch
global.fetch = vi.fn();

// Mock storage
const localStorageMock = {
  getItem: vi.fn(),
  setItem: vi.fn(),
  removeItem: vi.fn(),
  clear: vi.fn(),
};
Object.defineProperty(global, "sessionStorage", {
  value: localStorageMock,
});

describe("Scan Component", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it("renders scan form with input field", () => {
    render(
      <BrowserRouter>
        <Scan />
      </BrowserRouter>
    );

    const input = screen.getByPlaceholderText(/directory path/i);
    expect(input).toBeInTheDocument();
  });

  it("renders submit button", () => {
    render(
      <BrowserRouter>
        <Scan />
      </BrowserRouter>
    );

    const button = screen.getByRole("button", { name: /scan/i });
    expect(button).toBeInTheDocument();
  });

  it("disables input and button when loading", async () => {
    global.fetch = vi.fn(() =>
      new Promise((resolve) =>
        setTimeout(
          () =>
            resolve({
              ok: true,
              json: async () => ({
                startedAt: new Date().toISOString(),
                filesScanned: 0,
                items: [],
              }),
            }),
          100
        )
      )
    );

    render(
      <BrowserRouter>
        <Scan />
      </BrowserRouter>
    );

    const input = screen.getByPlaceholderText(/directory path/i) as HTMLInputElement;
    const button = screen.getByRole("button", { name: /scan/i }) as HTMLButtonElement;

    await userEvent.type(input, "C:/test");
    fireEvent.click(button);

    // Check if they become disabled during loading
    expect(input.disabled || button.disabled).toBe(true);
  });

  it("shows error message on failed scan", async () => {
    global.fetch = vi.fn(() =>
      Promise.resolve({
        ok: false,
        status: 400,
        json: async () => ({ error: "Invalid path" }),
      })
    );

    render(
      <BrowserRouter>
        <Scan />
      </BrowserRouter>
    );

    const input = screen.getByPlaceholderText(/directory path/i);
    const button = screen.getByRole("button", { name: /scan/i });

    await userEvent.type(input, "");
    fireEvent.click(button);

    await waitFor(() => {
      const errorMessage = screen.queryByText(/error|failed|invalid/i);
      expect(errorMessage).toBeTruthy();
    });
  });

  it("accepts directory path input", async () => {
    render(
      <BrowserRouter>
        <Scan />
      </BrowserRouter>
    );

    const input = screen.getByPlaceholderText(/directory path/i) as HTMLInputElement;
    await userEvent.type(input, "C:/projects/test");

    expect(input.value).toBe("C:/projects/test");
  });
});
